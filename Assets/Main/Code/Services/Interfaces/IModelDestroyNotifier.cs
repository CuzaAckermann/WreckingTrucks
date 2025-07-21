using System;
using System.Collections.Generic;

public interface IModelDestroyNotifier
{
    public event Action<Model> ModelDestroyRequested;

    public event Action<List<Model>> ModelsDestroyRequested;
}