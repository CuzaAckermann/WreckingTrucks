public class SelectedSignal<T> : EventBusSignal
{
    public SelectedSignal(T selectable)
    {
        Validator.ValidateNotNull(selectable);

        Selectable = selectable;
    }

    public T Selectable { get; private set; }
}