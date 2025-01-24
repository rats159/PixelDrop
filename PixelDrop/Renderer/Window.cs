using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using PixelDrop.Input;
using PixelDrop.Pixels;


namespace PixelDrop.Renderer;

public class Window(int width, int height)
    : GameWindow(GameWindowSettings.Default, new() { ClientSize = (width, height), Title = "PixelDrop" })
{
    private readonly World _world = new();


    protected override void OnLoad()
    {
        Mouse.Init(this);
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        Mouse.State = this.MouseState;
        Keyboard.State = this.KeyboardState;
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        this._world.Tick();
        this._world.Render();
        this.SwapBuffers();
    }

    protected override void OnUnload()
    {
        base.OnUnload();
        this._world.Dispose();
    }
    
    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
    }

    public void AddScroll(Action<MouseWheelEventArgs> cb)
    {
        this.MouseWheel += cb;
    }
}