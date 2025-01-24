using PixelDrop.Pixels.Data;

namespace PixelDrop.Pixels.Rules;

public static class RainbowRules
{
    public static void HueShift(int x, int y, World world, Pixel pixel)
    {
        if (pixel is not RainbowPixel pix)
            throw new ArgumentException("HueShift rule can only be applied to RainbowPixels");

        pix.Hue -= 1/720f;
        pix.Hue %= 1f;
    }
}