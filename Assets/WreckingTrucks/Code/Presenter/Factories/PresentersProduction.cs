using System;
using System.Collections.Generic;

public abstract class PresentersProduction
{
    private readonly Dictionary<Type, Func<Presenter>> _presentersFactories =
        new Dictionary<Type, Func<Presenter>>();

    public void Register<M, P>(Func<P> creator) where M : Model
                                                where P : Presenter
    {
        _presentersFactories[typeof(M)] = () => creator();
    }

    public Presenter CreatePresenter(Model model)
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
                if (_presentersFactories.TryGetValue(modelType, out Func<Presenter> creator))
                {
                    return creator();
                }
            }
        }

        throw new KeyNotFoundException($"No presenter factory for {model.GetType()}");
    }
}