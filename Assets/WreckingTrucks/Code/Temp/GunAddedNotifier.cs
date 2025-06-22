using System;

public class GunAddedNotifier : IModelAddedNotifier 
{
    public event Action<Model> ModelAdded;

    public void OnModelAdded(Gun gun)
    {
        ModelAdded?.Invoke(gun);
    }
}