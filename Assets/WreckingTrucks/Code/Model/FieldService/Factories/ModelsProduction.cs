using System;
using System.Collections.Generic;

public abstract class ModelsProduction
{
    private readonly Dictionary<Type, ModelFactory> _modelsFactories =
        new Dictionary<Type, ModelFactory>();

    public void AddFactory<T>(ModelFactory modelFactory) where T : Model
    {
        if (_modelsFactories.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"Factory is existing for {typeof(T)}");
        }

        _modelsFactories[typeof(T)] = modelFactory;
    }

    public Model CreateModel(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        Type modelType = model.GetType();

        foreach (Type type in _modelsFactories.Keys)
        {
            if (modelType == type)
            {
                if (_modelsFactories.TryGetValue(modelType, out ModelFactory factory))
                {
                    return factory.Create();
                }
            }
        }

        throw new KeyNotFoundException($"No model factory for {model.GetType()}");
    }
}