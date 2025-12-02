using System;

public class TruckGeneratorCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly ModelGeneratorSettings _modelGeneratorSettings;

    public TruckGeneratorCreator(ModelProductionCreator modelProductionCreator, ModelGeneratorSettings modelTypeGeneratorSettings)
    {
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _modelGeneratorSettings = modelTypeGeneratorSettings ?? throw new ArgumentNullException(nameof(modelTypeGeneratorSettings));
    }

    public TruckGenerator Create()
    {
        ModelProbabilitySettings modelProbabilitySettings = new ModelProbabilitySettings();
        TruckGenerator modelGenerator = new TruckGenerator(_modelProductionCreator.CreateTruckFactory(),
                                                           modelProbabilitySettings,
                                                           _modelGeneratorSettings.MinAmountProbabilityReduction,
                                                           _modelGeneratorSettings.MaxAmountProbabilityReduction);

        return modelGenerator;
    }
}