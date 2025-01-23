using OpenTK.Windowing.Common;
using PixelDrop.Pixels;
using PixelDrop.Renderer;

namespace PixelDrop;

internal static class Program
{
    public static void Main()
    {
        using Window window = new(World.WIDTH * World.PIXEL_SIZE, World.HEIGHT * World.PIXEL_SIZE);
        window.VSync = VSyncMode.On;
        window.Run();
    }
}