using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace PixelDrop.Input;

public static class Mouse
{
    private static Renderer.Window? _window;

    private static readonly List<Action<MouseWheelEventArgs>> ScrollQueue = [];

    public const MouseButton LEFT = MouseButton.Button1;
    public const MouseButton RIGHT = MouseButton.Button2;
    public const MouseButton MIDDLE = MouseButton.Button3;
    public const MouseButton BACK = MouseButton.Button4;
    public const MouseButton FORWARD = MouseButton.Button5;
    public static MouseState State { get; set; } = null!;

    public static void Init(Renderer.Window window)
    {
        Mouse._window = window;
        foreach (Action<MouseWheelEventArgs> cb in Mouse.ScrollQueue)
        {
            Mouse.RegisterScroll(cb);
        }

        Mouse.ScrollQueue.Clear();
    }

    public static void RegisterScroll(Action<int> cb)
    {
        Mouse.RegisterScroll(args => cb((int)args.OffsetY));
    }
    
    public static void RegisterScroll(Action<int,int> cb)
    {
        Mouse.RegisterScroll(args => cb((int)args.OffsetX,(int)args.OffsetY));
    }

    public static void RegisterScroll(Action<MouseWheelEventArgs> cb)
    {
        if (Mouse._window == null)
        {
            Mouse.QueueScrollCallback(cb);
        }
        else
        {
            Mouse._window.AddScroll(cb);
        }
    }

    private static void QueueScrollCallback(Action<MouseWheelEventArgs> cb)
    {
        Mouse.ScrollQueue.Add(cb);
    }
}