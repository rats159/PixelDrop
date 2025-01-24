namespace PixelDrop.Pixels;

public delegate Pixel PixelFactoryFunc(World world);

public readonly record struct PixelFactory(PixelFactoryFunc FactoryFunc, PixelType Type);