#version 330 core
in vec2 f_UV;

out vec4 FragColor;

uniform sampler2D u_Tex;

void main()
{
    FragColor = texture(u_Tex,f_UV);
}