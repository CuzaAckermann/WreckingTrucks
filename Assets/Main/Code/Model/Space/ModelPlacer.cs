public class ModelPlacer<M> where M : Model
{
    private readonly ModelFactory<M> _modelFactory;
    private readonly ModelSlot<M> _modelSlot;

    public ModelPlacer(ModelFactory<M> modelFactory,
                       ModelSlot<M> modelSlot)
    {
        Validator.ValidateNotNull(modelFactory);
        Validator.ValidateNotNull(modelSlot);

        _modelFactory = modelFactory;
        _modelSlot = modelSlot;
    }

    public void PlaceModel()
    {
        M model = _modelFactory.Create();

        model.SetColor(ColorType.Gray);

        _modelSlot.SetModel(model);
    }
}