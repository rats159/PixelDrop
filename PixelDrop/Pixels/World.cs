using OpenTK.Windowing.GraphicsLibraryFramework;
using PixelDrop.Input;
using PixelDrop.Pixels.Data;
using PixelDrop.Pixels.Rules;
using PixelDrop.Renderer;

namespace PixelDrop.Pixels;

public class World
{
    public const int PIXEL_SIZE = 2;
    public const int WIDTH = 800 / World.PIXEL_SIZE;
    public const int HEIGHT = 800 / World.PIXEL_SIZE;

    private Pixel[] _newGrid = new Pixel[World.HEIGHT * World.WIDTH];
    private Pixel[] _oldGrid = new Pixel[World.HEIGHT * World.WIDTH];
    private PixelRenderer? _pixelRenderer;

    private int _pixelIndex;

    private readonly PixelFactory[] _pixelFactories =
    [
        Dataless.OfType(PixelType.Sand),
        Dataless.OfType(PixelType.Water),
        Dataless.OfType(PixelType.SeaweedSeed),
        Dataless.OfType(PixelType.Static),
        Dataless.OfType(PixelType.Decay),
        Dataless.OfType(PixelType.Bricks),
        new((_,_,_) => new BirdPixel(PixelType.Bird),PixelType.Bird),
        new((_,_,_) => new RainbowPixel(PixelType.Rainbow),PixelType.Rainbow)
    ];

    private int _brushSize = 1;

    public World()
    {
        Mouse.RegisterScroll(delta =>
        {
            if (Keyboard.State.IsKeyDown(Keys.LeftShift))
            {
                this.ChangeBrushSize(delta);
            }
            else
            {
                this.ChangeSelectedPixel(delta);
            }
        });
        for (int i = 0; i < World.HEIGHT; i++)
        {
            for (int j = 0; j < World.WIDTH; j++)
            {
                this._newGrid[i * World.WIDTH + j] = new DatalessPixel(PixelType.Air);
                this._oldGrid[i * World.WIDTH + j] = new DatalessPixel(PixelType.Air);
            }
        }
    }

    private void ChangeBrushSize(int delta)
    {
        this._brushSize += delta;
        if (this._brushSize < 1)
        {
            this._brushSize = 1;
        }

        Console.WriteLine($"Brush size: {this._brushSize}");
    }

    private void ChangeSelectedPixel(int delta)
    {
        int numPixelTypes = this._pixelFactories.Length;
        this._pixelIndex += delta;

        // Python-style modulo, where negative indices wrap around;
        this._pixelIndex = (this._pixelIndex % numPixelTypes + numPixelTypes) % numPixelTypes;
        Console.WriteLine($"Now Drawing: {this._pixelFactories[this._pixelIndex].Type.Name}");
    }

    public void Spawn(int x, int y, PixelType type)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return;
        if (this._newGrid[y * World.WIDTH + x].type != PixelType.Air) return;

        this.Replace(x, y, type);
    }

    public void Replace(int x, int y, PixelType type)
    {
        this.Replace(x, y, new DatalessPixel(type));
    }

    public void Replace(int x, int y, Pixel pix)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return;

        this._oldGrid[y * World.WIDTH + x] = pix;
    }

    public void Swap(int x1, int y1, int x2, int y2)
    {
        (this._oldGrid[y1 * World.WIDTH + x1], this._oldGrid[y2 * World.WIDTH + x2]) = (
            this._oldGrid[y2 * World.WIDTH + x2], this._oldGrid[y1 * World.WIDTH + x1]);
    }

    public Pixel? Get(int x, int y)
    {
        if (x < 0 || x >= World.WIDTH || y < 0 || y >= World.HEIGHT) return null;
        return this._newGrid[y * World.WIDTH + x];
    }

    public PixelType? GetType(int x, int y)
    {
        return this.Get(x, y)?.type;
    }

    private void Draw()
    {
        int x = (int)Mouse.State.X / World.PIXEL_SIZE;
        int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
        for (int drawX = x; drawX < x + this._brushSize; drawX++)
        {
            for (int drawY = y; drawY < y + this._brushSize; drawY++)
            {
                int arrX = drawX - this._brushSize / 2;
                int arrY = drawY - this._brushSize / 2;
                this.Replace(arrX, arrY,
                    this._pixelFactories[this._pixelIndex].FactoryFunc(this,arrX,arrY));
            }
        }
    }

    private void Erase()
    {
        int x = (int)Mouse.State.X / World.PIXEL_SIZE;
        int y = (int)Mouse.State.Y / World.PIXEL_SIZE;
        for (int drawX = x; drawX < x + this._brushSize; drawX++)
        {
            for (int drawY = y; drawY < y + this._brushSize; drawY++)
            {
                int arrX = drawX - this._brushSize / 2;
                int arrY = drawY - this._brushSize / 2;
                if (arrX < 0 || arrX >= World.WIDTH || arrY < 0 || arrY >= World.HEIGHT) continue;

                this._newGrid[arrY * World.WIDTH + arrX] = new DatalessPixel(PixelType.Erase);
            }
        }
    }

    public void Tick()
    {
        for (int y = 0; y < World.HEIGHT; y++)
        {
            for (int x = 0; x < World.WIDTH; x++)
            {
                Pixel pixel = this._newGrid[y * World.WIDTH + x];
                foreach (PixelRule rule in pixel.type.Rules)
                {
                    rule(x, y, this, pixel);
                }
            }
        }

        this._newGrid = this._oldGrid;
        this._oldGrid = (Pixel[])this._oldGrid.Clone(); // TODO: Surely there's a better way

        if (Mouse.State.IsButtonDown(Mouse.LEFT))
        {
            this.Draw();
        }

        if (Mouse.State.IsButtonDown(Mouse.RIGHT))
        {
            this.Erase();
        }
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