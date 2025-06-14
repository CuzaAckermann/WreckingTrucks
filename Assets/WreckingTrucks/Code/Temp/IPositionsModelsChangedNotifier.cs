using System;
using System.Collections.Generic;

public interface IPositionsModelsChangedNotifier
{
    public event Action<List<Model>> TargetPositionsModelsChanged;
}