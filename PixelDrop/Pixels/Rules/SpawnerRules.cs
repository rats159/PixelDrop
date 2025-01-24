namespace PixelDrop.Pixels.Rules;

public static class SpawnerRules
{
    public static PixelRule MakeSpawner(PixelType type)
    {
        return (x, y, world,_) => world.Spawn(x,y+1,type);
    }
}