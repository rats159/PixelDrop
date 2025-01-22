using OpenTK.Windowing.Common;
using PixelDrop.Renderer;

namespace PixelDrop;

internal static class Program
{
    public static void Main()
    {
        using Window window = new(900, 900);
        window.VSync = VSyncMode.On;
        window.Run();
    }
}