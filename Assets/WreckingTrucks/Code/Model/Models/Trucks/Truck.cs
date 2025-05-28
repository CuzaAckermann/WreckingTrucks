using System;

public abstract class Truck : Model
{
    private Path _path;
    private Gun _gun;
    private Type DestroyableType;

    public Truck()
    {

    }

    public void SetDestroyableType<T>() where T : Block
    {
        DestroyableType = typeof(T);
    }
}