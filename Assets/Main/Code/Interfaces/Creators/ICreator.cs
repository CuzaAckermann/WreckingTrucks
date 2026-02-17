using System;

public interface ICreator<out T> where T : IDestroyable
{
    public event Action<T> Created;

    public T Create();
}