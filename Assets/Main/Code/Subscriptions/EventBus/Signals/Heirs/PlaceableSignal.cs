public class PlaceableSignal : EventBusSignal
{
    private readonly Model _model;

    public PlaceableSignal(Model model)
    {
        Validator.ValidateNotNull(model);

        _model = model;
    }

    public Model Model => _model;
}
