using System;

public class Level
{
    private readonly EventBus _eventBus;

    public Level(EventBus eventBus,
                 BlockField blockField,
                 CartrigeBoxField cartrigeBoxField,
                 ModelSlot planeSlot)
    {
        Validator.ValidateNotNull(eventBus, blockField, cartrigeBoxField, planeSlot);

        _eventBus = eventBus;

        BlockField = blockField;
        CartrigeBoxField = cartrigeBoxField;
        PlaneSlot = planeSlot;
    }

    public event Action StateChanged;

    public 

    public BlockField BlockField { get; }

    public CartrigeBoxField CartrigeBoxField { get; }

    public ModelSlot PlaneSlot { get; }

    public void Clear()
    {
        _eventBus.Invoke(new ClearedSignal<Level>());
    }

    public void Enable()
    {
        _eventBus.Invoke(new EnabledSignal<Level>());
    }

    public void Disable()
    {
        _eventBus.Invoke(new DisabledSignal<Level>());
    }
}