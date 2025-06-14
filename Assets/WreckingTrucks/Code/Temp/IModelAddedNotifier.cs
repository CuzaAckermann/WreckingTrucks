using System;

public interface IModelAddedNotifier
{
    public event Action<Model> ModelAdded;
}