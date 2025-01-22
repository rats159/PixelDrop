using OpenTK.Mathematics;
using PixelDrop.Renderer;

namespace PixelDrop.Pixels;

public class Pixel
{
    private static readonly Quad BaseQuad = new();

    public Quad Model => Pixel.BaseQuad;

    public PixelType Type { get; }

    private Vector2 _pos;

    public Pixel(Vector2 position, PixelType type)
    {
        this._pos = position;
        this.Type = type;
        this.UpdateMatrix();
    }

    public Matrix4 Transformation { get; private set; }

    public Vector2 Position => this._pos;

    public void PutAt(float x, float y)
    {
        this._pos.X = x;
        this._pos.Y = y;
        this.UpdateMatrix();
    }

    public void Move(float dx, float dy)
    {
        this._pos.X += dx;
        this._pos.Y += dy;
        this.UpdateMatrix();
    }

    private void UpdateMatrix()
    {
        this.Transformation = Matrix4.CreateTranslation(this._pos.X, this._pos.Y, 0);
    }
}