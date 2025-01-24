﻿namespace PixelDrop.Pixels.Rules;

public static class EraseRules
{
    public static void Erase(int x, int y, World world)
    {
        world.Replace(x,y,PixelType.Air);
    }
}