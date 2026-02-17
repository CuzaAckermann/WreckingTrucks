using System;

public abstract class ModelActivator<M> where M : Model
{
    protected readonly EventBus EventBus;

    public ModelActivator(EventBus eventBus)
    {
        EventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        EventBus.Subscribe<ClearedSignal<Level>>(Clear);

        EventBus.Subscribe<SelectedSignal<Model>>(AcrivateModel);
    }

    protected abstract void AcrivateModel(SelectedSignal<Model> selectedSignal);

    private void Clear(ClearedSignal<Level> _)
    {
        EventBus.Unsubscribe<ClearedSignal<Level>>(Clear);

        EventBus.Unsubscribe<SelectedSignal<Model>>(AcrivateModel);
    }
}