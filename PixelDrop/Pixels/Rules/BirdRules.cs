using PixelDrop.Pixels.Data;

namespace PixelDrop.Pixels.Rules;

public static class BirdRules
{
    public static void Fly(int x, int y, World world, Pixel pixel)
    {
        if (pixel is not BirdPixel bird) throw new ArgumentException("Fly rule cannot be applied to non-birds");

        if (!PixelType.Airs.Contains(world.GetType(x + bird.Dx, y)))
        {
            bird.Bounce();
        }
        
        world.Swap(x,y,x+bird.Dx,y);
    }
}