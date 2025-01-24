namespace PixelDrop.Pixels;

public delegate Pixel PixelFactoryFunc(World world,int x,int y);

public readonly record struct PixelFactory(PixelFactoryFunc FactoryFunc, PixelType Type);