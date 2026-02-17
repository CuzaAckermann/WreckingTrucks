public class ActivatedSignal<M> : EventBusSignal where M : Model
{
    public ActivatedSignal(M activatable)
    {
        Validator.ValidateNotNull(activatable);

        Activatable = activatable;
    }

    public M Activatable { get; private set; }
}