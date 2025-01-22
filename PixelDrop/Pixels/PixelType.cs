using OpenTK.Mathematics;
using PixelDrop.Pixels.Rules;
using PixelDrop.Renderer;

namespace PixelDrop.Pixels;

public class PixelType(string name, PixelRule[] rules, Vector3 color)
{
    public static readonly PixelType Sand = new("sand", [SandRules.Fall,SandRules.Pile], (255, 255, 0));
    public static readonly PixelType Water = new("water", [SandRules.Fall,WaterRules.Flow], (0, 0, 255));
    public static readonly PixelType Air = new("air", [], (0, 0, 0));

    public readonly Vector3 Color = color;
    public readonly string Name = name;
    public readonly PixelRule[] Rules = rules;
}