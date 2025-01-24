using OpenTK.Mathematics;

namespace PixelDrop.Pixels.Data;

public class BirdPixel(PixelType type, BirdData data) : Pixel(type, data)
{
    public override Vector3i GetColor()
    {
        return this.type.Color;
    }
    
    public void Bounce()
    {
        data.Bounce();
    }

    public int Dx => data.Dx;
}

public class BirdData : IPixelData
{
    public int Dx { get; private set; } = 1;

    public void Bounce()
    {
        this.Dx *= -1;
    }
}