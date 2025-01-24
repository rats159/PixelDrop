using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace PixelDrop.Pixels.Data;

public class RainbowPixel(PixelType type, int x, int y) : Pixel(type, new RainbowData(x, y))
{
    public override Vector3i GetColor()
    {
        return ((RainbowData)this.data).GetColor();
    }

    public float Hue
    {
        get => ((RainbowData)this.data).hue;
        set => ((RainbowData)this.data).hue = value;
    }
}

public class RainbowData(int x, int y) : IPixelData
{
    public float hue;

    public Vector3i GetColor()
    {
        return this.Hsl(this.hue % 1f, 1, .5f);
    }

    private Vector3i Hsl(float h, float s, float l)
    {
        float q = l < 0.5f ? l * (1 + s) : l + s - l * s;
        float p = 2 * l - q;

        float r = RainbowData.HueToRgb(p, q, h + 1f / 3f);
        float g = RainbowData.HueToRgb(p, q, h);
        float b = RainbowData.HueToRgb(p, q, h - 1f / 3f);

        if (r < 0) r = 0;
        if (g < 0) g = 0;
        if (b < 0) b = 0;
        // Convert to 0-255 range
        return ((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    private static float HueToRgb(float p, float q, float t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        if (t < 1f / 6f) return p + (q - p) * 6 * t;
        if (t < 1f / 2f) return q;
        if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6;
        return p;
    }
}