using System;

public interface IModel
{
    public event Action<IModel> InterfaceDestroyed;
}