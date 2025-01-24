using PixelDrop.Input;
using PixelDrop.Pixels.Rules;
using PixelDrop.Renderer;

namespace PixelDrop.Pixels;

public class World
{
    public const int PIXEL_SIZE = 10;
    public const int WIDTH = 800 / World.PIXEL_SIZE;
    public const int HEIGHT = 800 / World.PIXEL_SIZE;

    private PixelType[] _newGrid = new PixelType[World.HEIGHT * World.WIDTH];
    private PixelType[] _oldGrid = new PixelType[World.HEIGHT * World.WIDTH];
    private PixelRenderer? _pixelRenderer;

    private int _pixelIndex;
    private readonly PixelType[] _pixelTypes = [PixelType.Sand,PixelType.Water,PixelType.SeaweedSeed];


    public World()
    {
        Mouse.RegisterScroll(delta =>
        {
            int numPixelTypes = this._pixelTypes.Length;
            this._pixelIndex += delta;
            
            // Python-style modulo, where negative indices wrap around;
            this._pixelIndex = (this._pixelIndex % numPixelTypes + numPixelTypes) % numPixelTypes;
            Console.WriteLine(this._pixelIndex);
        });
        for (int i = 0; i < World.HEIGHT; i++)
        {
            for (int j = 0; j < World.WIDTH; j++)
            {
                this._newGrid[i * World.WIDTH + j] = PixelType.Air;
                this._oldGrid[i * World.WIDTH + j] = PixelType.Air;
            }
        }
    }

    public void Spawn(int x, int y, PixelType type)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return;
        if (this._newGrid[y * World.WIDTH + x] != PixelType.Air) return;

        this._newGrid[y * World.WIDTH + x] = type;
        this._oldGrid[y * World.WIDTH + x] = type;
    }
    
    public void Replace(int x, int y, PixelType type)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return;

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
        Console.WriteLine(this._pixelTypes[this._pixelIndex].Name);
        if (Mouse.State.IsButtonDown(Mouse.LEFT))
        {
            int x = (int)Mouse.State.X / World.PIXEL_SIZE;
            int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
            this.Spawn(x, y, this._pixelTypes[this._pixelIndex]);
        }
        
        if (Mouse.State.IsButtonDown(Mouse.RIGHT))
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
        this._oldGrid = (PixelType[])this._oldGrid.Clone(); // TODO: Surely there's a better way
    }

    public void Render()
    {
        this._pixelRenderer ??= new(new("Resources/shaders/shader.vert", "Resources/shaders/shader.frag"));
        this._pixelRenderer.Render(this._newGrid);
    }

    public void Dispose()
    {
        this._pixelRenderer?.Dispose();
    }
}