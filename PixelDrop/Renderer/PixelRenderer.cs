using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PixelDrop.Pixels;
using PixelDrop.Renderer.Shaders;
using PixelType = OpenTK.Graphics.OpenGL.PixelType;

namespace PixelDrop.Renderer;

public class PixelRenderer
{
    private readonly Shader _shader;
    private readonly int _textureId;
    private readonly byte[] _textureData;
    private readonly Quad _quad;

    public PixelRenderer(Shader shader)
    {
        this._textureData = new byte[World.WIDTH * World.HEIGHT * 3];
        this._quad = new();
        this._textureId = GL.GenTexture();
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2d,this._textureId);
        
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        this._shader = shader;
    }

    public void Render(Pixel[] world)
    {
        this.PrepareTexture(world);
        this.DrawQuad();
    }

    private void PrepareTexture(Pixel[] world)
    {
        for (int i = 0; i < world.Length; i++)
        {
            Vector3i color = world[i].GetColor();
            this._textureData[i * 3] = (byte)color.X;
            this._textureData[i * 3 + 1] = (byte)color.Y;
            this._textureData[i * 3 + 2] = (byte)color.Z;
        }
        
        GL.TexImage2D(TextureTarget.Texture2d, 0, InternalFormat.Rgb, World.WIDTH, World.HEIGHT, 0, PixelFormat.Rgb,
            PixelType.UnsignedByte, this._textureData);
    }

    private void DrawQuad()
    {
        this._shader.Use();
        GL.BindVertexArray(this._quad.VaoId);
        GL.EnableVertexAttribArray(0);
        GL.EnableVertexAttribArray(1);
        GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        GL.DisableVertexAttribArray(0);
        GL.DisableVertexAttribArray(1);
    }

    public void Dispose()
    {
        this._shader.Dispose();
    }
}