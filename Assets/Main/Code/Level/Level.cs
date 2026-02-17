using System;

public class Level
{
    private readonly EventBus _eventBus;

    public Level(EventBus eventBus,
                 BlockField blockField,
                 CartrigeBoxField cartrigeBoxField,
                 ModelSlot<Plane> planeSlot)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        BlockField = blockField ?? throw new ArgumentNullException(nameof(blockField));
        CartrigeBoxField = cartrigeBoxField ?? throw new ArgumentNullException(nameof(cartrigeBoxField));
        PlaneSlot = planeSlot ?? throw new ArgumentNullException(nameof(planeSlot));
    }

    public BlockField BlockField { get; }

    public CartrigeBoxField CartrigeBoxField { get; }

    public ModelSlot<Plane> PlaneSlot { get; }

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