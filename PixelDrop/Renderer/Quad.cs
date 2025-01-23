using OpenTK.Graphics.OpenGL;

namespace PixelDrop.Renderer;

public struct Quad
{
    private readonly float[] _vertices =
    [   // Pos      // UV
        -1f, -1f,   0,1,
        -1f,  1f,   0,0,
         1f, -1f,   1,1,
         1f,  1f,   1,0
    ];

    public Quad()
    {
        int vboId = GL.GenBuffer();
        this.VaoId = GL.GenVertexArray();
        GL.BindVertexArray(this.VaoId);

        GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
        GL.BufferData(BufferTarget.ArrayBuffer, this._vertices.Length * sizeof(float), this._vertices,
            BufferUsage.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
    }

    public int VaoId { get; }
}