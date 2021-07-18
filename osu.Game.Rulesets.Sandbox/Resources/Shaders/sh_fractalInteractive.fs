varying mediump vec2 v_TexCoord;
varying mediump vec4 v_TexRect;
varying mediump vec4 v_Colour;

uniform mediump float scale;
uniform vec2 cameraPosition;
uniform vec2 drawSize;

float distanceToMandelbrot(vec2 c)
{
    #if 1
    {
        float c2 = dot(c, c);
        // skip computation inside M1 - http://iquilezles.org/www/articles/mset_1bulb/mset1bulb.htm
        if( 256.0*c2*c2 - 96.0*c2 + 32.0*c.x - 3.0 < 0.0 ) return 0.0;
        // skip computation inside M2 - http://iquilezles.org/www/articles/mset_2bulb/mset2bulb.htm
        if( 16.0*(c2+2.0*c.x+1.0) - 1.0 < 0.0 ) return 0.0;
    }
    #endif

    float di = 1.0;
    vec2 z = vec2(0.0);
    float m2 = 0.0;
    vec2 dz = vec2(0.0);
    for (int i=0; i<300; i++)
    {
        if (m2 > 1024.0)
        {
            di = 0.0;
            break;
        }

        dz = 2.0*vec2(z.x*dz.x-z.y*dz.y, z.x*dz.y + z.y*dz.x) + vec2(1.0,0.0);        
        z = vec2( z.x*z.x - z.y*z.y, 2.0*z.x*z.y ) + c;
        m2 = dot(z,z);
    }

	float d = 0.5*sqrt(dot(z,z)/dot(dz,dz))*log(dot(z,z));

    if (di > 0.5)
        d=0.0;
        
    return d;
}

void main(void) {
    vec2 resolution = vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[0]);
    float x = v_TexCoord.x / resolution.x;
    float y = v_TexCoord.y / resolution.y;
    vec2 p = vec2(x, y) - 0.5;

	vec2 c = cameraPosition + p / scale;

    float d = distanceToMandelbrot(c * (drawSize / drawSize.y));
    
    //soft coloring based on distance
	d = clamp(pow(4.0 * d * scale, 0.2), 0.0, 1.0);
    
    gl_FragColor = vec4(vec3(d), v_Colour.w);
}
