using System;
using System.Collections.Generic;

public class PresentersProduction
{
    private readonly Dictionary<Type, Func<Model, Presenter>> _presenterCreators =
        new Dictionary<Type, Func<Model, Presenter>>();

    public void Register<M, P>(Func<M, P> creator)
        where M : Model
        where P : Presenter
    {
        _presenterCreators[typeof(M)] = model => creator((M)model);
    }

    public Presenter CreatePresenter(Model model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var modelType = model.GetType();

        while (modelType != null && modelType != typeof(Model))
        {
            if (_presenterCreators.TryGetValue(modelType, out var creator))
            {
                return creator(model);
            }

            modelType = modelType.BaseType;
        }

        throw new KeyNotFoundException($"No presenter for {model.GetType()}");
    }
}