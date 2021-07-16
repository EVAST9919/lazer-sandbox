varying mediump vec2 v_TexCoord;
varying mediump vec4 v_TexRect;
varying mediump vec4 v_Colour;

uniform mediump float scale;

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

	vec2 r = vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[0]);

	vec2 p = (2.0*v_TexCoord-r)/r.y;

    // animation	
	float tz = scale;
    float zoo = pow( 0.5, 13.0*tz );
	vec2 c = vec2(-0.05,.6805) + p*zoo;

    // distance to Mandelbrot
    float d = distanceToMandelbrot(c);
    
    // do some soft coloring based on distance
	d = clamp( pow(4.0*d/zoo,0.2), 0.0, 1.0 );
    
    vec3 col = vec3(d);
    
    gl_FragColor = vec4( col, v_Colour.w );
}
