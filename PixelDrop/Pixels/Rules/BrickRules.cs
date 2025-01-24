namespace PixelDrop.Pixels.Rules;

public static class BrickRules
{
    private static readonly PixelType[] Fallthrough = [PixelType.Air, PixelType.Water,PixelType.Erase, ];

    public static void Fall(int x, int y, World world)
    {
        PixelType? below = world.Get(x, y + 1);

        if (below == null || !BrickRules.CanFallthrough(below)) return;
        
        PixelType? left = world.Get(x-1, y);
        PixelType? right = world.Get(x+1, y);

        if (left == PixelType.Bricks) return;
        if (right == PixelType.Bricks) return;
        
        world.Swap(x, y, x, y + 1);
    }
    
    private static bool CanFallthrough(PixelType type)
    {
        for (int i = 0; i < BrickRules.Fallthrough.Length; i++)
        {
            if (BrickRules.Fallthrough[i] == type) return true;
        }

        return false;
    }
}