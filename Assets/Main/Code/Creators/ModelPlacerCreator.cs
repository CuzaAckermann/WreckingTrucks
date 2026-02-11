public class ModelPlacerCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;

    public ModelPlacerCreator(ModelProductionCreator modelProductionCreator)
    {
        Validator.ValidateNotNull(modelProductionCreator);

        _modelProductionCreator = modelProductionCreator;
    }

    public ModelPlacer<M> Create<M>(ModelSlot<M> modelSlot) where M : Model
    {
        return new ModelPlacer<M>(_modelProductionCreator.CreateFactory<M>(), modelSlot);
    }
}