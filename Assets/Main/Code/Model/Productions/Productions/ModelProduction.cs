using System;
using System.Collections.Generic;

public class ModelProduction<M> : IModelsProduction<M> where M : Model
{
    private readonly Dictionary<Type, IModelCreator<M>> _modelsFactories = new Dictionary<Type, IModelCreator<M>>();

    public void AddFactory<MType>(IModelCreator<M> modelFactory) where MType : M
    {
        Type modelType = typeof(MType);

        if (_modelsFactories.ContainsKey(modelType))
        {
            throw new InvalidOperationException($"{nameof(IModelCreator<M>)} for type '{modelType.Name}' is already added.");
        }

        _modelsFactories[modelType] = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));
    }

    public M CreateModel(Type typeModel)
    {
        if (typeModel == null)
        {
            throw new ArgumentNullException(nameof(typeModel));
        }

        if (_modelsFactories.TryGetValue(typeModel, out IModelCreator<M> factory) == false)
        {
            throw new KeyNotFoundException($"No factory registered for type '{typeModel.Name}'");
        }

        return factory.Create();
    }
}