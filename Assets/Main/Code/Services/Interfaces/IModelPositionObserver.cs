using System;
using System.Collections.Generic;

public interface IModelPositionObserver
{
    public event Action<Model> PositionChanged;

    public event Action<Model> PositionReached;

    public event Action<IModel> InterfacePositionChanged;

    public event Action<List<Model>> PositionsChanged;

    public event Action<List<IModel>> InterfacePositionsChanged;
}