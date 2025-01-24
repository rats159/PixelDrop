using PixelDrop.Pixels.Data;

namespace PixelDrop.Pixels.Rules;

public static class WaterRules
{
    public static void Flow(int x, int y, World world,Pixel _)
    {
        PixelType? left = world.GetType(x - 1, y);
        PixelType? right = world.GetType(x + 1, y);

        bool canGoLeft = left != null && left == PixelType.Air;
        bool canGoRight = right != null && right == PixelType.Air;

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