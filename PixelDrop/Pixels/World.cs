using PixelDrop.Input;
using PixelDrop.Pixels.Rules;
using PixelDrop.Renderer;

namespace PixelDrop.Pixels;

public class World
{
    public const int WIDTH = 400;
    public const int HEIGHT = 400;
    public const int PIXEL_SIZE = 2;

    private PixelType[] _newGrid = new PixelType[World.HEIGHT * World.WIDTH];
    private PixelType[] _oldGrid = new PixelType[World.HEIGHT * World.WIDTH];
    private PixelRenderer? _pixelRenderer;


    public World()
    {
        for (int i = 0; i < World.HEIGHT; i++)
        {
            for (int j = 0; j < World.WIDTH; j++)
            {
                this._newGrid[i * World.WIDTH + j] = PixelType.Air;
                this._oldGrid[i * World.WIDTH + j] = PixelType.Air;
            }
        }

        for (int i = 0; i < 11; i++)
        {
            this.Spawn(45, i * 2, PixelType.Sand);
        }
    }

    public void Spawn(int x, int y, PixelType type)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return;
        if (this._newGrid[y * World.WIDTH + x] != PixelType.Air) return;

        this._newGrid[y * World.WIDTH + x] = type;
        this._oldGrid[y * World.WIDTH + x] = type;
    }

    private void Unset(int x, int y)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return;
        if (this._newGrid[y * World.WIDTH + x] == PixelType.Air) return;

        this._oldGrid[y * World.WIDTH + x] = PixelType.Air;
    }

    public void Swap(int x1, int y1, int x2, int y2)
    {
        (this._oldGrid[y1 * World.WIDTH + x1], this._oldGrid[y2 * World.WIDTH + x2]) = (
            this._oldGrid[y2 * World.WIDTH + x2], this._oldGrid[y1 * World.WIDTH + x1]);
    }

    public PixelType? Get(int x, int y)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return null;
        return this._newGrid[y * World.WIDTH + x];
    }

    public void Tick()
    {
        DateTime start = DateTime.Now;
        if (Mouse.State.IsButtonDown(Mouse.LEFT))
        {
            int x = (int)Mouse.State.X / World.PIXEL_SIZE;
            int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
            this.Spawn(x, y, PixelType.Sand);
        }

        if (Mouse.State.IsButtonDown(Mouse.RIGHT))
        {
            int x = (int)Mouse.State.X / World.PIXEL_SIZE;
            int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
            this.Spawn(x, y, PixelType.Water);
        }

        if (Mouse.State.IsButtonDown(Mouse.MIDDLE))
        {
            int x = (int)Mouse.State.X / World.PIXEL_SIZE;
            int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
            this.Spawn(x, y, PixelType.SandSpawner);
        }

        if (Mouse.State.IsButtonDown(Mouse.BACK))
        {
            int x = (int)Mouse.State.X / World.PIXEL_SIZE;
            int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
            this.Spawn(x, y, PixelType.WaterSpawner);
        }

        if (Mouse.State.IsButtonDown(Mouse.FORWARD))
        {
            int x = (int)Mouse.State.X / World.PIXEL_SIZE;
            int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
            this.Unset(x, y);
        }

        for (int y = 0; y < World.HEIGHT; y++)
        {
            for (int x = 0; x < World.WIDTH; x++)
            {
                PixelType pixel = this._newGrid[y * World.WIDTH + x];
                foreach (PixelRule rule in pixel.Rules)
                {
                    rule(x, y, this);
                }
            }
        }

        this._newGrid = this._oldGrid;
        this._oldGrid = (PixelType[])this._oldGrid.Clone();
        DateTime end = DateTime.Now;
        Console.WriteLine($"All computation done in {(end - start).TotalMilliseconds}ms");
    }

    public void Render()
    {
        DateTime start = DateTime.Now;
        this._pixelRenderer ??= new(new("Resources/shaders/shader.vert", "Resources/shaders/shader.frag"));
        this._pixelRenderer.Render(this._newGrid);
        DateTime end = DateTime.Now;
        Console.WriteLine($"All rendering done in {(end - start).TotalMilliseconds}ms");
    }

    public void Dispose()
    {
        this._pixelRenderer?.Dispose();
    }
}