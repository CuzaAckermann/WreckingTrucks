using System;
using System.Collections.Generic;

public class RowGenerator
{
    private readonly List<RowGenerationStrategy> _rowGenerationStrategies;
    private readonly List<ColorType> _colorTypes;
    private readonly Random _random;

    public RowGenerator(List<RowGenerationStrategy> rowGenerationStrategies, List<ColorType> colorTypes)
    {
        _rowGenerationStrategies = rowGenerationStrategies ?? throw new ArgumentNullException(nameof(rowGenerationStrategies));
        _colorTypes = colorTypes ?? throw new ArgumentNullException(nameof(colorTypes));
        _random = new Random();
    }

    public List<ColorType> Create(int amountInRow)
    {
        return _rowGenerationStrategies[_random.Next(0, _rowGenerationStrategies.Count)].Generate(_colorTypes, amountInRow);
    }
}