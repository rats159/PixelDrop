namespace PixelDrop.Pixels.Rules;

public static class DecayRules
{
    private static readonly PixelType[] RotStable = [PixelType.Air, PixelType.Rot, PixelType.Decay,];

    private static readonly PixelType[] UnDecayable =
    [
        PixelType.Air, PixelType.Decay,
        PixelType.Rot,PixelType.Erase, 
    ];

    public static void Decay(int x, int y, World world)
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

            PixelType? neighbor = world.Get(x + dx, y + dy);
            if (neighbor == null || DecayRules.IsUnDecayable(neighbor)) continue;
            world.Replace(x + dx, y + dy, PixelType.Decay);
        }
    }

    public static void Rot(int x, int y, World world)
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
            PixelType? neighbor = world.Get(x + dx, y + dy);
            if (neighbor == null) continue;

            if (!DecayRules.StabilizesRot(neighbor)) stable = false;
        }

        if (stable) world.Replace(x, y, PixelType.Air);
    }

    private static bool StabilizesRot(PixelType type)
    {
        for (int i = 0; i < DecayRules.RotStable.Length; i++)
        {
            if (DecayRules.RotStable[i] == type) return true;
        }

        return false;
    }

    private static bool IsUnDecayable(PixelType type)
    {
        for (int i = 0; i < DecayRules.UnDecayable.Length; i++)
        {
            if (DecayRules.UnDecayable[i] == type) return true;
        }

        return false;
    }
}