public class ModelPlacerCreator
{
    private readonly Production _production;

    public ModelPlacerCreator(Production production)
    {
        Validator.ValidateNotNull(production);

        _production = production;
    }

    public ModelPlacer<M> Create<M>(ModelSlot modelSlot) where M : Model
    {
        return new ModelPlacer<M>(_production, modelSlot);
    }
}