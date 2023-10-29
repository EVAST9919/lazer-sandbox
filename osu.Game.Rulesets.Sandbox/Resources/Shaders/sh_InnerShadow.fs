#ifndef INNER_SHADOW_FS
#define INNER_SHADOW_FS

#include "sh_Utils.h"
#include "sh_Masking.h"
#include "sh_TextureWrapping.h"

layout(location = 2) in mediump vec2 v_TexCoord;

layout(set = 0, binding = 0) uniform lowp texture2D m_Texture;
layout(set = 0, binding = 1) uniform lowp sampler m_Sampler;

layout(location = 0) out vec4 o_Colour;

void main(void) 
{
	highp vec2 topLeftOffset = g_MaskingRect.xy - v_MaskingPosition;
	highp vec2 bottomRightOffset = v_MaskingPosition - g_MaskingRect.zw;

	highp vec2 distanceFromShrunkRect = max(
		bottomRightOffset + vec2(g_CornerRadius),
		topLeftOffset + vec2(g_CornerRadius));

	highp float maxDist = max(distanceFromShrunkRect.x, distanceFromShrunkRect.y);
	highp float minDist = min(distanceFromShrunkRect.x, distanceFromShrunkRect.y);

	lowp float a = 0.0;
	const float size = 10.0;

	if (maxDist > 0.0)
	{
		highp float dst = minDist < 0.0 ? maxDist : distance(vec2(-minDist, 0.0), vec2(0.0, maxDist));
		highp float dstFromEdge = clamp(max(g_CornerRadius, size) - dst, 0.0, size);
		a = smoothstep(size, -size, dstFromEdge);
	}

	vec2 wrappedCoord = wrap(v_TexCoord, v_TexRect);
	vec4 texCol = wrappedSampler(wrappedCoord, v_TexRect, m_Texture, m_Sampler, -0.9);
	texCol.a *= a;
    o_Colour = getRoundedColor(texCol, wrappedCoord);
}

#endif