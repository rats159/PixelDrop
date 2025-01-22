using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PixelDrop.Pixels;
using PixelDrop.Renderer.Shaders;

namespace PixelDrop.Renderer;

public class PixelRenderer
{
    private readonly Shader _shader;

    private readonly Matrix4 _projection;
    private readonly Matrix4 _view;

    public PixelRenderer(Shader shader)
    {
        this._shader = shader;

        this._projection = Matrix4.CreateOrthographicOffCenter(0, World.Width, World.Height, 0, -1, 1);
        this._view = Matrix4.LookAt(0, 0, 0, 0, 0, -1, 0, 1, 0);
        this._shader.Use();
        this._shader.Load("u_Proj", this._projection);
        this._shader.Load("u_View", this._view);
    }

    public void Render(Dictionary<Pixels.PixelType, List<Pixel>> grouped)
    {
        foreach ((Pixels.PixelType type, List<Pixel> pixels) in grouped)
        {
            this.BeginType(type);
            foreach (Pixel pixel in pixels)
            {
                this.RenderPixel(pixel);
            }
        }
    }

    private void BeginType(Pixels.PixelType type)
    {
        this._shader.Load("u_Color", type.Color);
    }

    private void RenderPixel(Pixel pixel)
    {
        Quad model = pixel.Model;
        this._shader.Use();

        GL.BindVertexArray(model.VaoId);

        GL.EnableVertexAttribArray(0);

        Matrix4 transform = pixel.Transformation;

        this._shader.Load("u_Transformation", transform);
        // Console.WriteLine(this._projection * this._view * transform * new Vector4(0, 0, 0f, 1.0f));

        GL.DrawElements(PrimitiveType.Triangles, model.Indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.DisableVertexAttribArray(0);
    }

    public void Dispose()
    {
        this._shader.Dispose();
    }
}