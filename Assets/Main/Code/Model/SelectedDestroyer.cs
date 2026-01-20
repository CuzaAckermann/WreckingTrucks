using System;

public class SelectedDestroyer<M> where M : Model
{
    private readonly EventBus _eventBus;

    public SelectedDestroyer(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _eventBus.Subscribe<ClearedSignal<Game>>(Clear);
        _eventBus.Subscribe<SelectedSignal>(DestroySelected);
    }

    private void Clear(ClearedSignal<Game> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Game>>(Clear);
        _eventBus.Unsubscribe<SelectedSignal>(DestroySelected);
    }

    private void DestroySelected(SelectedSignal selectedSignal)
    {
        if (selectedSignal.Selectable is not M model)
        {
            return;
        }

        model.Destroy();
    }
}