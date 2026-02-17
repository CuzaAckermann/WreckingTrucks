using System;
using System.Collections.Generic;

public class ModelColorGeneratorCreator
{
    private readonly ModelGeneratorSettings _modelGeneratorSettings;

    public ModelColorGeneratorCreator(ModelGeneratorSettings modelTypeGeneratorSettings)
    {
        _modelGeneratorSettings = modelTypeGeneratorSettings ?? throw new ArgumentNullException(nameof(modelTypeGeneratorSettings));
    }

    public ModelColorGenerator Create(Field field,
                                      IReadOnlyList<ColorType> colorTypes)
    {
        ModelProbabilitySettings modelProbabilitySettings = new ModelProbabilitySettings();
        ColorGenerator colorGenerator = new ColorGenerator(modelProbabilitySettings,
                                                           _modelGeneratorSettings.MinAmountProbabilityReduction,
                                                           _modelGeneratorSettings.MaxAmountProbabilityReduction);

        ModelColorGenerator modelColorGenerator = new ModelColorGenerator(colorGenerator);
        modelColorGenerator.SetColorTypes(colorTypes);
        modelColorGenerator.PrepareRecords(field);

        return modelColorGenerator;
    }
}