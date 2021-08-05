#include "sh_Utils.h"

varying mediump vec2 v_TexCoord;
varying vec4 v_TexRect;

const float gradient = 0.003f;

#define PI 3.1415926538

uniform lowp sampler2D m_Sampler;

vec2 toLensSpace(vec2 uv)
{
	uv = uv * 2.0 - 1.0;
	vec2 offset = abs( uv.yx ) / vec2( 6.0, 4.0 );
	uv = uv + uv * offset * offset;
	uv = uv * 0.5 + 0.5;
	return uv;
}

float smoothLensEdge(vec2 uv)
{
	float x = gradient;
	float y = gradient;
	
	if (uv.x < gradient)
		x = uv.x;
	else if (uv.x > 1.0 - gradient)
		x = 1.0 - uv.x;

	if (uv.y < gradient)
		y = uv.y;
	else if (uv.y > 1.0 - gradient)
		y = 1.0 - uv.y;

	return min(x, y) / gradient;
}

bool insideLens(vec2 uv)
{
	return uv.x > 0.0 && uv.x < 1.0 && uv.y > 0.0 && uv.y < 1.0;
}

vec4 scanlines(vec4 colour, vec2 uv)
{
    float scanline = clamp(0.95 + 0.05 * cos(PI * (uv.y + 0.008 * 0.1) * 240.0 * 1.0), 0.0, 1.0);
    float grille = 0.85 + 0.15 * clamp(1.5 * cos(PI * uv.x * 640.0 * 1.0), 0.0, 1.0);
	return vec4(vec3(colour.rgb * scanline * grille * 1.2), colour.a);
}

void main(void)
{
	vec2 resolution = vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[1] - v_TexRect[0]);
	vec2 uv = v_TexCoord / resolution;
	vec2 lensUV = toLensSpace(uv);

	vec4 col = vec4(vec3(0.0), 1.0);

	if (insideLens(lensUV))
	{
		vec4 lensCol = toSRGB(texture2D(m_Sampler, lensUV * resolution));
		lensCol = scanlines(lensCol, lensUV);
		float smoothEdge = smoothLensEdge(lensUV);
		col = vec4(lensCol.rgb * smoothEdge, clamp(lensCol.a + 1.0 - smoothEdge, 0.0, 1.0));
	}

	gl_FragColor = col;
}
