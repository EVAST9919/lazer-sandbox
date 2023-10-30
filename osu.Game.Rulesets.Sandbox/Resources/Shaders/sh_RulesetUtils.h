#define PI 3.1415926538

vec4 transparentBlack(vec3 c)
{
	float alpha = c.r * c.g * c.b;
	return vec4(c, alpha < 0.01 ? 0 : alpha);
}

vec2 rotate(float length, vec2 origin, float angle)
{
    float rad = angle * PI / 180.0;
    return vec2(cos(rad), sin(rad)) * length + origin;
}

vec2 rotateAround(vec2 point, vec2 origin, float angle)
{
    float rad = angle * PI / 180.0;
    float s = sin(rad);
    float c = cos(rad);

    point -= origin;
    
    return vec2(point.x * c - point.y * s, point.x * s + point.y * c) + origin;
}

highp float dstToLine(highp vec2 start, highp vec2 end, highp vec2 pixelPos)
{
    highp float lineLength = distance(end, start);

    if (lineLength < 0.001)
        return distance(pixelPos, start);

    highp vec2 a = (end - start) / lineLength;
    highp vec2 closest = clamp(dot(a, pixelPos - start), 0.0, distance(end, start)) * a + start; // closest point on a line from given position
    return distance(closest, pixelPos);
}

float map(float value, float minValue, float maxValue, float minEndValue, float maxEndValue)
{
    return (value - minValue) / (maxValue - minValue) * (maxEndValue - minEndValue) + minEndValue;
}

bool almostEqual(float v1, float v2)
{
    return abs(v1.x - v2.x) < 0.00001;
}

bool almostEqual(vec2 p1, vec2 p2)
{
    return almostEqual(p1.x, p2.x) && almostEqual(p1.y, p2.y);
}

vec2 getShaderTexturePosition(vec2 value, vec2 texRes, vec2 topLeft)
{
	return topLeft - fract(value) * texRes;
}
