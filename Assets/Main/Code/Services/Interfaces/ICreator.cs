using System;

public interface ICreator<out T> where T : IDestroyable
{
    public event Action<T> Created;

    public Type CreatableType => typeof(T);

    public T Create();
}