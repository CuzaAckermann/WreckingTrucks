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

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(Enable);
        _eventBus.Subscribe<DisabledSignal<GameSignalEmitter>>(Disable);
    }

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<GameSignalEmitter>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<GameSignalEmitter>>(Disable);
    }

    private void Enable(EnabledSignal<GameSignalEmitter> _)
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelAdded);
    }

    private void Disable(DisabledSignal<GameSignalEmitter> _)
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