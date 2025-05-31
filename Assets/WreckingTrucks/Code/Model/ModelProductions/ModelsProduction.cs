using System;
using System.Collections.Generic;

public class ModelsProduction<M, MF> where M : Model
                                     where MF : ModelFactory<M>
{
    private readonly Dictionary<Type, MF> _modelsFactories =
        new Dictionary<Type, MF>();

    public void AddFactory<MType>(MF modelFactory) where MType : M
    {
        Type modelType = typeof(MType);

        if (_modelsFactories.ContainsKey(modelType))
        {
            throw new InvalidOperationException($"{nameof(MF)} for type '{modelType.Name}' is already added.");
        }

        _modelsFactories[modelType] = modelFactory;
    }

    public M Create(Type typeModel)
    {
        if (typeModel == null)
        {
            throw new ArgumentNullException(nameof(typeModel));
        }

        foreach (Type type in _modelsFactories.Keys)
        {
            if (typeModel == type)
            {
                if (_modelsFactories.TryGetValue(typeModel, out MF factory))
                {
                    return factory.Create();
                }
            }
        }

        throw new KeyNotFoundException($"No {nameof(MF)} for type '{typeModel.GetType()}'");
    }
}