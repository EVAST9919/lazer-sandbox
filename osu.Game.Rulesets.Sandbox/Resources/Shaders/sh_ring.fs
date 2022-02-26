#include "sh_Utils.h"
#include "sh_RulesetUtils.h"
#include "sh_TextureWrapping.h"

varying lowp vec4 v_Colour;
varying mediump vec2 v_TexCoord;
varying mediump vec4 v_TexRect;

uniform lowp sampler2D m_Sampler;
uniform mediump float progress;
uniform mediump float innerRadius;
uniform mediump float texel;

bool outsideProgressSector(vec2 pixelPos, float progress)
{
    if (progress == 0.0)
        return true;

    float angle = atan(0.5 - pixelPos.y, 0.5 - pixelPos.x) - PI / 2.0;

    if (angle < 0.0)
        angle += 2.0 * PI;

    return angle > 2.0 * PI * progress;
}

float getVerticalAlpha(float dstFromCentre, float texel, float innerRadius)
{
    float innerBorder = 0.5 * (1.0 - innerRadius);
    
    if (dstFromCentre < innerBorder)
        return 1.0 - (innerBorder - max(dstFromCentre, innerBorder - texel)) / texel;

    float outerBorder = 0.5;
        
    if (dstFromCentre > outerBorder - texel)
        return (outerBorder - min(dstFromCentre, outerBorder)) / texel;

    return 1.0;
}

void main(void)
{
    if (progress == 0.0 || innerRadius == 0.0)
    {
        gl_FragColor = vec4(0.0);
        return;
    }

    vec2 resolution = v_TexRect.zw - v_TexRect.xy;
    vec2 pixelPos = v_TexCoord / resolution;

    float alphaMultiplier = getVerticalAlpha(distance(pixelPos, vec2(0.5)), texel, innerRadius);

    if (alphaMultiplier == 0.0)
    {
        gl_FragColor = vec4(0.0);
        return;
    }

    if (outsideProgressSector(pixelPos, progress))
    {
        // cap 1
        vec2 startLineStart = vec2(0.5, texel);
        vec2 startLineEnd = vec2(0.5, innerRadius * 0.5);

        // cap 2
        vec2 endLineStart = rotate(0.5 - texel, vec2(0.5), progress * 360.0 - 90.0);
        vec2 endLineEnd = rotate(0.5 - innerRadius * 0.5, vec2(0.5), progress * 360.0 - 90.0);

        float dstToCap = min(distanceToLine(startLineStart, startLineEnd, pixelPos), distanceToLine(endLineStart, endLineEnd, pixelPos));
        alphaMultiplier = (1.0 - min(dstToCap, texel) / texel);

        if (dstToCap > texel || alphaMultiplier == 0.0)
        {
            gl_FragColor = vec4(0.0);
            return;
        }
    }

    gl_FragColor = toSRGB(v_Colour * wrappedSampler(wrap(v_TexCoord, v_TexRect), v_TexRect, m_Sampler, -0.9)) * vec4(vec3(1.0), alphaMultiplier);
}
