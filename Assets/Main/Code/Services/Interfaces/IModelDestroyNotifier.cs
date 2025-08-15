using System;
using System.Collections.Generic;

public interface IModelDestroyNotifier
{
    public event Action<Model> ModelDestroyRequested;

    public event Action<IModel> InterfaceModelDestroyRequested;

    public event Action<IReadOnlyList<Model>> ModelsDestroyRequested;

    public event Action<IReadOnlyList<IModel>> InterfaceModelsDestroyRequested;
}