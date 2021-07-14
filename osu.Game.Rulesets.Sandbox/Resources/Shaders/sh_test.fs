varying mediump vec2 v_TexCoord;
varying mediump vec4 v_TexRect;

uniform mediump float time;

void main(void) {

	vec2 r = vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[0]);

	vec3 c;
	float l,z=time;
	for(int i=0;i<3;i++) {
		vec2 uv,p=v_TexCoord/r;
		uv=p;
		p-=.5;
		p.x*=r.x/r.y;
		z+=.07;
		l=length(p);
		uv+=p/l*(sin(z)+1.)*abs(sin(l*9.-z*2.));
		c[i]=.01/length(abs(mod(uv,1.)-.5));
	}

	vec3 col = vec3(c/l);
	float alpha = col.r * col.g * col.b;
	gl_FragColor = vec4(col,alpha < 0.01 ? 0 : alpha);
}