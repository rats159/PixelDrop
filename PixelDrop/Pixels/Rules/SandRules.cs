namespace PixelDrop.Pixels.Rules;

public static class SandRules
{
    private static readonly PixelType[] Fallthrough = [PixelType.Air, PixelType.Water];

    public static void Fall(int x, int y, World world)
    {
        PixelType? below = world.Get(x, y + 1);

        if (below == null || !SandRules.CanFallthrough(below)) return;

        world.Swap(x, y, x, y + 1);
    }

    public static void Pile(int x, int y, World world)
    {
        PixelType? leftDown = world.Get(x - 1, y + 1);
        PixelType? rightDown = world.Get(x + 1, y + 1);

        bool canGoLeft = leftDown != null && SandRules.CanFallthrough(leftDown);
        bool canGoRight = rightDown != null && SandRules.CanFallthrough(rightDown);

        if (!canGoLeft && !canGoRight) return;

        int offset = canGoLeft switch
        {
            true when canGoRight => Random.Shared.NextSingle() >= 0.5f ? -1 : 1,
            true => -1,
            _ => 1
        };

        world.Swap(x, y, x + offset, y + 1);
    }

    private static bool CanFallthrough(PixelType type)
    {
        for (int i = 0; i < SandRules.Fallthrough.Length; i++)
        {
            if (SandRules.Fallthrough[i] == type) return true;
        }

        return false;
    }
}