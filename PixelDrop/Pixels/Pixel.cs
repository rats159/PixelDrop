using OpenTK.Mathematics;
using PixelDrop.Pixels.Data;

namespace PixelDrop.Pixels;

public abstract class Pixel(PixelType type, IPixelData data)
{
    public readonly PixelType type = type;
    private readonly IPixelData data = data;

    public abstract Vector3i GetColor();
}