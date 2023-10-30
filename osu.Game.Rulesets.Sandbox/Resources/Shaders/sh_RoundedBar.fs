#ifndef ROUNDED_BAR_FS
#define ROUNDED_BAR_FS

#include "sh_Utils.h"
#include "sh_Masking.h"
#include "sh_RulesetUtils.h"

layout(location = 2) in mediump vec2 v_TexCoord;

layout(set = 0, binding = 0) uniform lowp texture2D m_Texture;
layout(set = 0, binding = 1) uniform lowp sampler m_Sampler;

layout(location = 0) out vec4 o_Colour;

void main(void) 
{
    const float pixelSize = 0.0000003;

    highp vec2 resolution = v_TexRect.wz - v_TexRect.yx;

    if (resolution.x < pixelSize * 2.0)
    {
        o_Colour = getRoundedColor(vec4(1.0), v_TexCoord);
        return;
    }

    highp vec2 coord = v_TexCoord.yx;
    highp vec2 halfSize = resolution * 0.5;

    if (coord.y > halfSize.y)
        coord.y = resolution.y - coord.y;

    highp float dst = coord.y < halfSize.x ? distance(coord, vec2(halfSize.x)) : abs(coord.x - halfSize.x);

    lowp float a = smoothstep(halfSize.x, halfSize.x - pixelSize, dst);
    o_Colour = getRoundedColor(vec4(vec3(1.0), a), v_TexCoord);
}

#endif
