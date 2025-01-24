namespace PixelDrop.Pixels.Rules;

public static class DecayRules
{
    // Rot won't disappear if only surrounded by these
    private static readonly PixelType[] RotStable = [PixelType.Air, PixelType.Rot, PixelType.Decay];
    
    // Decay won't spread to these
    private static readonly PixelType[] UnDecayable =
    [
        PixelType.Air, PixelType.Decay,
        PixelType.Rot, PixelType.Erase,
    ];
    
    private static readonly (int, int)[] Dirs =
    [
        (0, 1),
        (0, -1),
        (1, 0),
        (-1, 0)
    ];

    public static void Decay(int x, int y, World world, Pixel _)
    {
        if (Random.Shared.NextSingle() <= 0.25)
        {
            world.Replace(x, y, PixelType.Rot);
        }

        foreach ((int dx, int dy) in DecayRules.Dirs)
        {
            if (Random.Shared.NextSingle() <= 0.75) continue;

            PixelType? neighbor = world.GetType(x + dx, y + dy);
            
            if (neighbor is null || DecayRules.UnDecayable.Contains(neighbor)) continue;
            
            world.Replace(x + dx, y + dy, PixelType.Decay);
        }
    }

    public static void Rot(int x, int y, World world, Pixel _)
    {
        if (Random.Shared.NextSingle() <= 0.75) return;

        bool unstable = true;

        foreach ((int dx, int dy) in DecayRules.Dirs)
        {
            PixelType? neighbor = world.GetType(x + dx, y + dy);
            if (neighbor == null) continue;

            if (!DecayRules.RotStable.Contains(neighbor)) unstable = false;
        }

        if (unstable) world.Replace(x, y, PixelType.Air);
    }
}