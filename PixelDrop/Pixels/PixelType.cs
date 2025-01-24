using OpenTK.Mathematics;
using PixelDrop.Pixels.Rules;
using PixelDrop.Renderer;

namespace PixelDrop.Pixels;

public record PixelType(Vector3i Color, string Name, PixelRule[] Rules)
{
    public static readonly PixelType Sand = new((255, 255, 0), "sand", [SandRules.Fall, SandRules.Pile]);
    public static readonly PixelType Water = new((0, 0, 255), "water", [SandRules.Fall, WaterRules.Flow]);
    public static readonly PixelType Erase = new((255, 255, 255), "erase", [EraseRules.Erase]);

    public static readonly PixelType SandSpawner =
        new((255, 255, 255), "sand_spawner", [SpawnerRules.MakeSpawner(PixelType.Sand)]);

    public static readonly PixelType WaterSpawner =
        new((255, 255, 255), "water_spawner", [SpawnerRules.MakeSpawner(PixelType.Water)]);

    public static readonly PixelType Air = new((0, 0, 20), "air", []);
    public static readonly PixelType SeaweedSeed = new((68, 38, 8), "seaweed_seed", [SeaweedRules.Fall]);
    public static readonly PixelType Seaweed = new((0, 128, 0), "seaweed", [SeaweedRules.Grow]);
    public static readonly PixelType SeaweedCap = new((0, 64, 0), "seaweed_cap", []);
}