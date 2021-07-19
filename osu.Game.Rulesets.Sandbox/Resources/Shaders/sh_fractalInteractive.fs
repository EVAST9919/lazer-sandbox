varying mediump vec2 v_TexCoord;
varying mediump vec4 v_TexRect;
varying mediump vec4 v_Colour;

uniform mediump float scale;
uniform vec2 cameraPosition;
uniform vec2 drawSize;

const int maxIterations = 300;

int distanceToMandelbrot(vec2 c)
{
    float x = 0.0;
    float y = 0.0;
    float x2 = 0.0;
    float y2 = 0.0;

    int i = 0;

    while (x2 + y2 < 4 && i < maxIterations)
    {
        y = 2 * x * y + c.y;
        x = x2 - y2 + c.x;
        x2 = x * x;
        y2 = y * y;
        i++;
    }

    return i;
}

void main(void) {
    vec2 resolution = vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[0]);
    float x = v_TexCoord.x / resolution.x;
    float y = v_TexCoord.y / resolution.y;
    vec2 p = vec2(x, y) - 0.5;

	vec2 c = cameraPosition + p / scale;
    int d = distanceToMandelbrot(c * (drawSize / drawSize.y));    
    float col = float(d) / maxIterations;
    gl_FragColor = vec4(vec3(col), v_Colour.w);
}
