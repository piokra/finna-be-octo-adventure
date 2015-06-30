
#version 150 core
in vec3 pass_Color;
out vec4 out_Color;
uniform vec3 uRGB;

void main(void) {

	out_Color = vec4(uRGB, 1.0);
}