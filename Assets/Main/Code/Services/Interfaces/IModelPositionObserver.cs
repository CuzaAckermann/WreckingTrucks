using System;
using System.Collections.Generic;

public interface IModelPositionObserver
{
    public event Action<Model> PositionChanged;

    public event Action<List<Model>> PositionsChanged;
}