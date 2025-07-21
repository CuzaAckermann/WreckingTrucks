using System;
using System.Collections.Generic;

public class PresenterProduction : IModelPresenterCreator
{
    private readonly Dictionary<Type, IPresenterCreator> _presenterCreators;

    public PresenterProduction()
    {
        _presenterCreators = new Dictionary<Type, IPresenterCreator>();
    }

    public void AddFactory<M>(IPresenterCreator presenterCreator) where M : Model
    {
        Type modelType = typeof(M);

        if (_presenterCreators.ContainsKey(modelType))
        {
            throw new InvalidOperationException($"{nameof(IPresenterCreator)} is existing for {modelType}");
        }

        _presenterCreators[modelType] = presenterCreator;
    }

    public IPresenter GetPresenter(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        Type modelType = model.GetType();

        if (_presenterCreators.TryGetValue(modelType, out IPresenterCreator presenterCreator))
        {
            return presenterCreator.Create();
        }

        throw new KeyNotFoundException($"No {nameof(IPresenterCreator)} for {model.GetType()}");
    }
}