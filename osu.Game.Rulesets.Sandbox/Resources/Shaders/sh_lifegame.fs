varying mediump vec2 v_TexCoord;
varying mediump vec4 v_TexRect;
varying mediump vec4 v_Colour;

struct Data
{
    bool[100] State; 
};

uniform Data data;

void main(void) {

	vec2 r = vec2(v_TexRect[2] - v_TexRect[0], v_TexRect[3] - v_TexRect[0]);

    int x = int(v_TexCoord.x / r.x) * 10;
    int y = int(v_TexCoord.x / r.y) * 10;
    int index = y * 10 + x;
    bool alive = data.State[index];

    gl_FragColor = vec4(vec3(alive ? 1.0 : 0.0), v_Colour.w);
}
