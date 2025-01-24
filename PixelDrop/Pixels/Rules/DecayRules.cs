namespace PixelDrop.Pixels.Rules;

public static class DecayRules
{
    private static readonly PixelType[] RotStable = [PixelType.Air, PixelType.Rot, PixelType.Decay,];

    private static readonly PixelType[] UnDecayable =
    [
        PixelType.Air, PixelType.Decay,
        PixelType.Rot, PixelType.Erase,
    ];

    public static void Decay(int x, int y, World world, Pixel _)
    {
        (int, int)[] dirs =
        [
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0)
        ];

        if (Random.Shared.NextSingle() <= 0.25)
        {
            world.Replace(x, y, PixelType.Rot);
        }

        foreach ((int dx, int dy) in dirs)
        {
            if (Random.Shared.NextSingle() <= 0.75)
            {
                continue;
            }

            PixelType? neighbor = world.GetType(x + dx, y + dy);
            if (neighbor == null || DecayRules.UnDecayable.Contains(neighbor)) continue;
            world.Replace(x + dx, y + dy, PixelType.Decay);
        }
    }

    public static void Rot(int x, int y, World world, Pixel _)
    {
        if (Random.Shared.NextSingle() <= 0.75)
        {
            return;
        }

        (int, int)[] dirs =
        [
            (0, 1),
            (0, -1),
            (1, 0),
            (-1, 0)
        ];

        bool stable = true;

        foreach ((int dx, int dy) in dirs)
        {
            PixelType? neighbor = world.GetType(x + dx, y + dy);
            if (neighbor == null) continue;

            if (!DecayRules.RotStable.Contains(neighbor)) stable = false;
        }

        if (stable) world.Replace(x, y, PixelType.Air);
    }
}