using System;

public interface IDestroyable
{
    public event Action<IDestroyable> DestroyedIDestroyable;

    public void Destroy();
}