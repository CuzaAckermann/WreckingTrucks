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

        _eventBus.Subscribe<ClearedSignal<Game>>(Clear);

        _eventBus.Subscribe<EnabledSignal<Game>>(Enable);
        _eventBus.Subscribe<DisabledSignal<Game>>(Disable);
    }

    private void Clear(ClearedSignal<Game> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Game>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<Game>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<Game>>(Disable);
    }

    private void Enable(EnabledSignal<Game> _)
    {
        _eventBus.Subscribe<CreatedSignal<Model>>(OnModelAdded);
    }

    private void Disable(DisabledSignal<Game> _)
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