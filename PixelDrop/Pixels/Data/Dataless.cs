using OpenTK.Mathematics;

namespace PixelDrop.Pixels.Data;

public class DatalessPixel(PixelType type) : Pixel(type,Dataless.Instance)
{
    public override Vector3i GetColor()
    {
        return this.type.Color;
    }
}

public class Dataless : IPixelData
{
    public static PixelFactory OfType(PixelType type)
    {
        return new(_ => new DatalessPixel(type),type);
    }
    public static Dataless Instance { get; } = new();
    
    private Dataless()
    {
    }
}