public class CreatedSignal<T> : EventBusSignal
{
    public CreatedSignal(T creatable)
    {
        Validator.ValidateNotNull(creatable);

        Creatable = creatable;
    }

    public T Creatable { get; private set; }
}