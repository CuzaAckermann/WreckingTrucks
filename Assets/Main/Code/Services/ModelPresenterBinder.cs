using System;

public class ModelPresenterBinder
{
    private readonly ApplicationStateStorage _applicationStateStorage;

    private readonly ModelProduction _modelProduction;
    private readonly IModelPresenterCreator _modelPresenterCreator;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinder(ApplicationStateStorage applicationStateStorage,
                                ModelProduction modelProduction,
                                IModelPresenterCreator modelPresenterCreators,
                                PresenterPainter presenterPainter)
    {
        Validator.ValidateNotNull(applicationStateStorage, modelProduction, modelPresenterCreators, presenterPainter);

        _applicationStateStorage = applicationStateStorage;
        _modelProduction = modelProduction;
        _modelPresenterCreator = modelPresenterCreators;
        _presenterPainter = presenterPainter;

        _applicationStateStorage.OnDestroyApplicationState.Triggered += Clear;

        _applicationStateStorage.OnEnableApplicationState.Triggered += Enable;
        _applicationStateStorage.OnDisableApplicationState.Triggered += Disable;
    }

    private void Clear()
    {
        _applicationStateStorage.OnDestroyApplicationState.Triggered -= Clear;

        _applicationStateStorage.OnEnableApplicationState.Triggered -= Enable;
        _applicationStateStorage.OnDisableApplicationState.Triggered -= Disable;
    }

    private void Enable()
    {
        _modelProduction.ModelCreated += BindModelToPresenter;
    }

    private void Disable()
    {
        _modelProduction.ModelCreated -= BindModelToPresenter;
    }

    private void BindModelToPresenter(Model model)
    {
        if (_modelPresenterCreator.TryGetPresenter(model, out Presenter presenter) == false)
        {
            return;
        }

        presenter.Bind(model);
        _presenterPainter.Paint(presenter);
    }
}