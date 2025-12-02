using System;

public interface IModelCreator<out M> where M : Model
{
    public event Action<M> ModelCreated;

    public M Create();
}