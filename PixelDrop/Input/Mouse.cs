using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PixelDrop.Input;

public static class Mouse
{
    public const MouseButton LEFT = MouseButton.Button1;
    public const MouseButton RIGHT = MouseButton.Button2;
    public const MouseButton MIDDLE = MouseButton.Button3;
    public const MouseButton BACK = MouseButton.Button4;
    public const MouseButton FORWARD = MouseButton.Button5;
    public static MouseState State { get; set; } = null!;
}