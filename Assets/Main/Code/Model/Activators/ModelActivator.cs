using System;

public abstract class ModelActivator<M> where M : Model
{
    protected readonly EventBus EventBus;

    public ModelActivator(EventBus eventBus)
    {
        EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        EventBus.Subscribe<ClearedSignal<GameWorld>>(Clear);
        EventBus.Subscribe<SelectedSignal>(AcrivateModel);
    }

    protected abstract void AcrivateModel(SelectedSignal selectedSignal);

    private void Clear(ClearedSignal<GameWorld> _)
    {
        EventBus.Unsubscribe<ClearedSignal<GameWorld>>(Clear);
        EventBus.Unsubscribe<SelectedSignal>(AcrivateModel);
    }
}