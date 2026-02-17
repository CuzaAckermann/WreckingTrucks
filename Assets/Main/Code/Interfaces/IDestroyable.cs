using System;

public interface IDestroyable
{
    public event Action<IDestroyable> Destroyed;

    public void Destroy();
}