using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using PixelDrop.Pixels;
using PixelDrop.Renderer.Shaders;
using PixelType = PixelDrop.Pixels.PixelType;

namespace PixelDrop.Renderer;

public class PixelRenderer
{
    private readonly Shader _shader;
    
    private readonly Dictionary<PixelType, List<Pixel>> _groups = [];

    public PixelRenderer(Shader shader)
    {
        this._shader = shader;

        Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, World.WIDTH, World.HEIGHT, 0, -1, 1);
        Matrix4 view = Matrix4.LookAt(0, 0, 0, 0, 0, -1, 0, 1, 0);
        this._shader.Use();
        this._shader.Load("u_Proj", projection);
        this._shader.Load("u_View", view);
    }

    public void Render(Pixel[] world)
    {
        this.MakeGroups(world);
        foreach ((PixelType type, List<Pixel> pixels) in this._groups)
        {
            if(type == PixelType.Air) continue;
            
            this.BeginType(type);
            foreach (Pixel pixel in pixels)
            {
                this.RenderPixel(pixel);
            }
        }
    }

    private void MakeGroups(Pixel[] world)
    {
        foreach (List<Pixel> typedGroup in this._groups.Values)
        {
            typedGroup.Clear();
        }
        
        foreach (Pixel p in world)
        {
            if (!this._groups.TryGetValue(p.Type, out List<Pixel>? value))
            {
                value = [];
                this._groups[p.Type] = value;
            }

            value.Add(p);    
        }
    }

    private void BeginType(PixelType type)
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

        GL.DrawElements(PrimitiveType.Triangles, model.Indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.DisableVertexAttribArray(0);
    }

    public void Dispose()
    {
        this._shader.Dispose();
    }
}