#version 150 core

in vec3 in_Position;
out vec3 pass_Color;
uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main(void) {
	gl_Position = vec4(in_Position, 1.0);
	pass_Color = vec3(0,0,0);
}