using System;

public class ModelFinalizerCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;

    public ModelFinalizerCreator(ModelProductionCreator modelProductionCreator)
    {
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
    }

    public ModelFinalizer Create()
    {
        return new ModelFinalizer(_modelProductionCreator.CreateModelProduction());
    }
}