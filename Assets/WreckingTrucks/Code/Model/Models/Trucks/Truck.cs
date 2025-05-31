using System;

public abstract class Truck : Model
{
    private Gun _gun;
    private Type DestroyableType;

    public void SetDestroyableType<T>() where T : Block
    {
        DestroyableType = typeof(T);
    }
}