using OpenTK.Platform;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PixelDrop.Input;
using PixelDrop.Pixels.Rules;
using PixelDrop.Renderer;

namespace PixelDrop.Pixels;

public class World
{
    public const int Width = 100;
    public const int Height = 100;
    public const int PixelSize = 9;
    
    private Pixel[] _newGrid = new Pixel[World.Height*World.Width];
    private Pixel[] _oldGrid = new Pixel[World.Height*World.Width];
    private Dictionary<PixelType, List<Pixel>> ByType { get; } = [];
    private PixelRenderer? _pixelRenderer;


    public World()
    {
        for (int i = 0; i < World.Height; i++)
        {
            for (int j = 0; j < World.Width; j++)
            {
                this._newGrid[i*World.Width+j] = new((j, i), PixelType.Air);
                this._oldGrid[i*World.Width+j] = new((j, i), PixelType.Air);
            }
        }
        
        for(int i = 0; i < 11; i++)
        {
            this.Spawn(45, i*2, PixelType.Sand);
        }
    }

    public void Spawn(int x, int y, PixelType type)
    {
        if (x < 0 || x >= World.Width || y < 0 || y >= World.Height) return;
        if (this._newGrid[y * World.Width + x].Type != PixelType.Air) return;
        
        Pixel px = new((x, y), type);
        this._newGrid[y * World.Width + x] = px;
        this._oldGrid[y * World.Width + x] = px;

        if (type == PixelType.Air) return;

        if (!this.ByType.TryGetValue(type, out List<Pixel>? list))
        {
            list = [];
            this.ByType[type] = list;
        }

        list.Add(px);
    }

    public void Swap(int x1, int y1, int x2, int y2)
    {
        (this._oldGrid[y1 * World.Width + x1], this._oldGrid[y2 * World.Width + x2]) = (this._oldGrid[y2 * World.Width + x2], this._oldGrid[y1 * World.Width + x1]);
        this._oldGrid[y1 * World.Width + x1].PutAt(x1, y1);
        this._oldGrid[y2 * World.Width+x2].PutAt(x2, y2);
    }

    public Pixel? Get(int x, int y)
    {
        if (x < 0 || x >= World.Width || y < 0 || y >= World.Height) return null;
        return this._newGrid[y *World.Width + x];
    }

    public void Tick()
    {
        if (Mouse.State.IsButtonDown(Mouse.Left))
        {
            int x = (int) Mouse.State.X / World.PixelSize;
            int y = (int) Mouse.State.Y / World.PixelSize;
            this.Spawn(x,y,PixelType.Sand);
        }
        
        if (Mouse.State.IsButtonDown(Mouse.Right))
        {
            int x = (int) Mouse.State.X / World.PixelSize;
            int y = (int) Mouse.State.Y / World.PixelSize;
            this.Spawn(x,y,PixelType.Water);
        }
        
        for (int y = 0; y < World.Height; y++)
        {
            for (int x = 0; x < World.Width; x++)
            {
                Pixel pixel = this._newGrid[y * World.Width + x];
                foreach (PixelRule rule in pixel.Type.Rules)
                {
                    rule(x, y, this);
                }
            }
        }   

        this._newGrid = this._oldGrid;
        this._oldGrid = (Pixel[])this._oldGrid.Clone();
    }

    public void Render()
    {
        this._pixelRenderer ??= new(new("Resources/shaders/shader.vert", "Resources/shaders/shader.frag"));
        this._pixelRenderer.Render(this.ByType);
    }

    public void Dispose()
    {
        this._pixelRenderer?.Dispose();
    }
}