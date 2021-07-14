vec4 transparentBlack(vec3 c)
{
	float alpha = c.r * c.g * c.b;
	return vec4(c, alpha < 0.01 ? 0 : alpha);
}
