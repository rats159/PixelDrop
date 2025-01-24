using PixelDrop.Pixels.Data;

namespace PixelDrop.Pixels.Rules;

public static class BirdRules
{
    public static void Fly(int x, int y, World world, Pixel pixel)
    {
        if (pixel is not BirdPixel bird) throw new ArgumentException("Fly rule cannot be applied to non-birds");

        if (world.GetType(x + bird.Dx, y) != PixelType.Air)
        {
            bird.Bounce();
        }
        
        world.Swap(x,y,x+bird.Dx,y);
    }
}