using System;

public interface IModelSource
{
    public event Action<Model> ModelAdded;
}