using System;

public class SelectedSignal : EventBusSignal
{
    private readonly Model _selectable;

    public SelectedSignal(Model selectable)
    {
        _selectable = selectable ?? throw new ArgumentNullException(nameof(selectable));
    }

    public Model Selectable => _selectable;
}