using System;
using System.Collections.Generic;

public abstract class PresentersProduction<M> where M : Model
{
    private readonly Dictionary<Type, PresenterFactory> _presentersFactories =
        new Dictionary<Type, PresenterFactory>();

    public void AddFactory<MType>(PresenterFactory presenterFactory) where MType : M
    {
        Type modelType = typeof(MType);

        if (_presentersFactories.ContainsKey(modelType))
        {
            throw new InvalidOperationException($"{nameof(PresenterFactory)} is existing for {modelType}");
        }

        _presentersFactories[modelType] = presenterFactory;
    }

    public Presenter CreatePresenter(M model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        Type modelType = model.GetType();

        foreach (Type type in _presentersFactories.Keys)
        {
            if (modelType == type)
            {
                if (_presentersFactories.TryGetValue(modelType, out PresenterFactory factory))
                {
                    return factory.Create();
                }
            }
        }

        throw new KeyNotFoundException($"No {nameof(PresenterFactory)} for {model.GetType()}");
    }
}