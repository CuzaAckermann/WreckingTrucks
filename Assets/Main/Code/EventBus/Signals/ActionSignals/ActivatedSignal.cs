using System;

public class ActivatedSignal<M> : EventBusSignal where M : Model
{
    private readonly M _activatable;

    public ActivatedSignal(M activatable)
    {
        _activatable = activatable ?? throw new ArgumentNullException(nameof(activatable));
    }

    public M Activatable => _activatable;
}