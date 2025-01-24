namespace PixelDrop.Pixels.Rules;

public static class SeaweedRules
{
    public static void Fall(int x, int y, World world,Pixel _)
    {
        PixelType? below = world.GetType(x, y + 1);

        if (below == null) return;

        if (!SandRules.CanFallthrough(below))
        {
            if (below != PixelType.Sand)
            {
                world.Replace(x, y, PixelType.Air);
            }
            else world.Replace(x, y, PixelType.Seaweed);
        }
        else
        {
            world.Swap(x, y, x, y + 1);
        }
    }

    public static void Grow(int x, int y, World world,Pixel _)
    {
        PixelType? above = world.GetType(x, y - 1);

        if (above == null) return;

        if (above == PixelType.Water)
        {
            world.Replace(x, y - 1, Random.Shared.NextSingle() <= 0.25f ? PixelType.SeaweedCap : PixelType.Seaweed);
        }
    }
}