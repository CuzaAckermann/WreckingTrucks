using System;
using System.Collections.Generic;

public class ModelsProduction<M, MF> : IModelsProduction where M : Model
                                                         where MF : ModelFactory<M>
{
    private readonly Dictionary<Type, MF> _modelsFactories = new Dictionary<Type, MF>();

    public void AddFactory<MType>(MF modelFactory) where MType : M
    {
        Type modelType = typeof(MType);

        if (_modelsFactories.ContainsKey(modelType))
        {
            throw new InvalidOperationException($"{nameof(MF)} for type '{modelType.Name}' is already added.");
        }

        _modelsFactories[modelType] = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));
    }

    public List<Model> CreateModels(List<Type> typeModels)
    {
        if (typeModels == null)
        {
            throw new ArgumentNullException(nameof(typeModels));
        }

        List<Model> models = new List<Model>(typeModels.Count);

        for (int i = 0; i < typeModels.Count; i++)
        {
            models.Add(Create(typeModels[i]));
        }

        return models;
    }

    private M Create(Type typeModel)
    {
        if (typeModel == null)
        {
            throw new ArgumentNullException(nameof(typeModel));
        }

        if (_modelsFactories.TryGetValue(typeModel, out MF factory) == false)
        {
            throw new KeyNotFoundException($"No factory registered for type '{typeModel.Name}'");
        }

        return factory.Create();
    }
}