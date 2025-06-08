using System;
using System.Collections.Generic;

public class PresentersProduction<M> : IModelPresenterSource where M : Model
{
    private readonly Dictionary<Type, IPresenterCreator> _presenterCreators;
    public PresentersProduction()
    {
        _presenterCreators = new Dictionary<Type, IPresenterCreator>();
    }

    public void AddFactory<MType>(IPresenterCreator presenterCreator) where MType : M
    {
        Type modelType = typeof(MType);

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