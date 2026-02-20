public class ModelPlacer<M> where M : Model
{
    private readonly Production _production;
    private readonly ModelSlot _modelSlot;

    public ModelPlacer(Production production,
                       ModelSlot modelSlot)
    {
        Validator.ValidateNotNull(production, modelSlot);

        _production = production;
        _modelSlot = modelSlot;
    }

    public void PlaceModel()
    {
        if (_production.TryCreate(out M requiredElement) == false)
        {
            return;
        }

        requiredElement.SetColor(ColorType.Gray);

        _modelSlot.SetModel(requiredElement);
    }
}