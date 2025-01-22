using OpenTK.Graphics.OpenGL;

namespace PixelDrop.Renderer;

public class Quad
{
    private readonly float[] _vertices =
    [
        0f, 0f,
        1f, 0f,
        0f, 1f,
        1f, 1f,
    ];

    public Quad()
    {
        int vboId = GL.GenBuffer();
        this.VaoId = GL.GenVertexArray();
        GL.BindVertexArray(this.VaoId);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
        GL.BufferData(BufferTarget.ArrayBuffer, this._vertices.Length * sizeof(float), this._vertices,
            BufferUsage.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        int eboId = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboId);
        GL.BufferData(BufferTarget.ElementArrayBuffer, this.Indices.Length * sizeof(uint), this.Indices,
            BufferUsage.StaticDraw);
    }

    public int VaoId { get; }

    public uint[] Indices { get; } =
    [
        2, 1, 0,
        2, 3, 1
    ];
}