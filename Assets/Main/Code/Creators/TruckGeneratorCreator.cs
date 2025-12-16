using System;
using System.Collections.Generic;

public class TruckGeneratorCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly ModelGeneratorSettings _modelGeneratorSettings;

    public TruckGeneratorCreator(ModelProductionCreator modelProductionCreator, ModelGeneratorSettings modelTypeGeneratorSettings)
    {
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _modelGeneratorSettings = modelTypeGeneratorSettings ?? throw new ArgumentNullException(nameof(modelTypeGeneratorSettings));
    }

    public TruckGenerator Create(Field field,
                                 IReadOnlyList<ColorType> colorTypes)
    {
        ModelProbabilitySettings modelProbabilitySettings = new ModelProbabilitySettings();
        ColorGenerator colorGenerator = new ColorGenerator(modelProbabilitySettings,
                                                           _modelGeneratorSettings.MinAmountProbabilityReduction,
                                                           _modelGeneratorSettings.MaxAmountProbabilityReduction);
        TruckGenerator truckGenerator = new TruckGenerator(_modelProductionCreator.CreateTruckFactory(),
                                                           colorGenerator);
        truckGenerator.SetColorTypes(colorTypes);
        truckGenerator.PrepareRecords(field);

        return truckGenerator;
    }
}