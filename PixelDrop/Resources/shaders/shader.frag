#version 330 core
in vec2 f_UV;

out vec4 FragColor;

uniform sampler2D u_Tex;

void main()
{
    FragColor = vec4(texture(u_Tex,f_UV).xyz,1);
}