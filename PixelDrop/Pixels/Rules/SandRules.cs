namespace PixelDrop.Pixels.Rules;

public static class SandRules
{
    public static void Fall(int x, int y, World world,Pixel _)
    {
        PixelType? below = world.GetType(x, y + 1);

        if (below == null || !PixelType.NonSolid.Contains(below)) return;

        world.Swap(x, y, x, y + 1);
    }

    public static void Pile(int x, int y, World world,Pixel _)
    {
        PixelType? leftDown = world.GetType(x - 1, y + 1);
        PixelType? rightDown = world.GetType(x + 1, y + 1);

        bool canGoLeft = leftDown != null && PixelType.NonSolid.Contains(leftDown);
        bool canGoRight = rightDown != null && PixelType.NonSolid.Contains(rightDown);

        if (!canGoLeft && !canGoRight) return;

        int offset = canGoLeft switch
        {
            true when canGoRight => Random.Shared.NextSingle() >= 0.5f ? -1 : 1,
            true => -1,
            _ => 1
        };

        world.Swap(x, y, x + offset, y + 1);
    }

    // public static bool CanFallthrough(PixelType type)
    // {
    //     for (int i = 0; i < SandRules.Fallthrough.Length; i++)
    //     {
    //         if (SandRules.Fallthrough[i] == type) return true;
    //     }
    //
    //     return false;
    // }
}