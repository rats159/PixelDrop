using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PixelDrop.Input;

public static class Mouse
{
    public const MouseButton Left = MouseButton.Button1;
    public const MouseButton Right = MouseButton.Button2;
    public static MouseState State { get; set; } = null!;
}