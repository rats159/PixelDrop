#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 aUV;

out vec2 f_UV;

void main()
{
    f_UV = aUV;
    gl_Position = vec4(aPosition, 0.5,1.0);
}