using System;
using System.Collections.Generic;

public class RowGeneratorCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly List<RowGenerationStrategy> _rowGenerationStrategies;
    private readonly List<ColorType> _colorTypes;

    public RowGeneratorCreator(ModelProductionCreator modelProductionCreator,
                               List<ColorType> colorType)
    {
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _colorTypes = colorType ?? throw new ArgumentNullException(nameof(colorType));

        _rowGenerationStrategies = new List<RowGenerationStrategy>()
        {
            new RowWithRandomPeriodGenerator(2, 2),
            new RowWithFixedPeriodGenerator(2),
            new RowWithPeriodicTypesGenerator(3, 3),
            new RowWithRandomTypesGenerator()
        };
    }

    public RowColorGenerator Create(int amountLayers, int amountColumns)
    {
        return new RowColorGenerator(_rowGenerationStrategies,
                                _colorTypes,
                                amountLayers,
                                amountColumns);
    }
}