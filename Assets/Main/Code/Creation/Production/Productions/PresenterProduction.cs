using System;
using System.Collections.Generic;

public class PresenterProduction : IModelPresenterCreator
{
    private readonly Dictionary<Type, IPresenterCreator> _presenterCreators;

    public PresenterProduction()
    {
        _presenterCreators = new Dictionary<Type, IPresenterCreator>();
    }

    public event Action<Presenter> Created;

    public void AddFactory<M>(IPresenterCreator presenterCreator) where M : Model
    {
        Type modelType = typeof(M);

        if (_presenterCreators.ContainsKey(modelType))
        {
            throw new InvalidOperationException($"{nameof(IPresenterCreator)} is existing for {modelType}");
        }

        _presenterCreators[modelType] = presenterCreator;
    }

    public bool TryGetPresenter(Model model, out Presenter presenter)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        Type modelType = model.GetType();

        if (_presenterCreators.TryGetValue(modelType, out IPresenterCreator presenterCreator))
        {
            presenter = presenterCreator.CreatePresenter();

            Created?.Invoke(presenter);

            return true;
        }

        presenter = null;

        return false;

        //throw new KeyNotFoundException($"No {nameof(IPresenterCreator)} for {model.GetType()}");
    }
}