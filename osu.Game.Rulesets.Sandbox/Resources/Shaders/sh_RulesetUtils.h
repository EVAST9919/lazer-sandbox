vec4 transparentBlack(vec3 c)
{
	float alpha = c.r * c.g * c.b;
	return vec4(c, alpha < 0.01 ? 0 : alpha);
}

float map(float value, float minValue, float maxValue, float minEndValue, float maxEndValue)
{
    return (value - minValue) / (maxValue - minValue) * (maxEndValue - minEndValue) + minEndValue;
}
