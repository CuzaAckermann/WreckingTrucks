using System;

public class ModelPresenterBinder
{
    private readonly EventBus _eventBus;

    private readonly IModelPresenterCreator _modelPresenterCreator;
    private readonly PresenterPainter _presenterPainter;

    public ModelPresenterBinder(EventBus eventBus,
                                IModelPresenterCreator modelPresenterCreators,
                                PresenterPainter presenterPainter)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _modelPresenterCreator = modelPresenterCreators ?? throw new ArgumentNullException(nameof(modelPresenterCreators));
        _presenterPainter = presenterPainter ? presenterPainter : throw new ArgumentNullException(nameof(presenterPainter));

        _eventBus.Subscribe<GameClearedSignal>(Clear);

        _eventBus.Subscribe<GameStartedSignal>(Enable);
        _eventBus.Subscribe<GameEndedSignal>(Disable);
    }

    private void Clear(GameClearedSignal _)
    {
        _eventBus.Unsubscribe<GameClearedSignal>(Clear);

        _eventBus.Unsubscribe<GameStartedSignal>(Enable);
        _eventBus.Unsubscribe<GameEndedSignal>(Disable);
    }

    private void Enable(GameStartedSignal _)
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelAdded);
    }

    private void Disable(GameEndedSignal _)
    {
        _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelAdded);
    }

    private void OnModelAdded(CreatedSignal<Model> modelSignal)
    {
        Model model = modelSignal.Creatable;

        if (_modelPresenterCreator.TryGetPresenter(model, out Presenter presenter) == false)
        {
            return;
        }

        presenter.Bind(model);
        _presenterPainter.Paint(presenter);
    }
}