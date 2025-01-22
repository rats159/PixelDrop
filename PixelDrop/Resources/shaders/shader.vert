#version 330 core
layout (location = 0) in vec2 aPosition;

uniform mat4 u_Transformation;
uniform mat4 u_Proj;
uniform mat4 u_View;

void main()
{
    gl_Position = u_Proj * u_View * u_Transformation * vec4(aPosition, 0.5,1.0);
}