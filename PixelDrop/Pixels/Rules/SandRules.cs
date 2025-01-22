namespace PixelDrop.Pixels.Rules;

public static class SandRules
{
    public static readonly PixelType[] Fallthrough = [PixelType.Air, PixelType.Water];

    public static void Fall(int x, int y, World world)
    {
        Pixel? below = world.Get(x, y + 1);
        if (SandRules.Fallthrough.Contains(below?.Type))
        {
            world.Swap(x, y, x, y + 1);
        }
    }

    public static void Pile(int x, int y, World world)
    {
        Pixel? leftDown = world.Get(x - 1, y + 1);
        Pixel? rightDown = world.Get(x + 1, y + 1);

        bool canGoLeft = SandRules.Fallthrough.Contains(leftDown?.Type);
        bool canGoRight = SandRules.Fallthrough.Contains(rightDown?.Type);

        if (!canGoLeft && !canGoRight) return;

        int offset = canGoLeft switch
        {
            true when canGoRight => Random.Shared.NextSingle() >= 0.5f ? -1 : 1,
            true => -1,
            _ => 1
        };

        world.Swap(x, y, x + offset, y + 1);
    }
}