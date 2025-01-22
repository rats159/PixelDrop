namespace PixelDrop.Pixels.Rules;

public static class WaterRules
{
    public static void Flow(int x, int y, World world)
    {
        Pixel? left = world.Get(x - 1, y);
        Pixel? right = world.Get(x + 1, y);

        bool canGoLeft = left?.Type == PixelType.Air;
        bool canGoRight = right?.Type == PixelType.Air;

        if (!canGoLeft && !canGoRight) return;

        int offset = canGoLeft switch
        {
            true when canGoRight => Random.Shared.NextSingle() >= 0.5f? -1 : 1,
            true => -1,
            _ => 1
        };
        
        world.Swap(x, y, x + offset, y);
    }
}