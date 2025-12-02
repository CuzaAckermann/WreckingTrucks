using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NonstopGameBlockFieldSettingsCreator
{
    private readonly List<GenerationStrategy> _generationStrategies;
    private readonly List<ColorType> _uniqueColors;

    public NonstopGameBlockFieldSettingsCreator(List<GenerationStrategy> generationStrategies,
                                                List<ColorType> uniqueColors)
    {
        _generationStrategies = generationStrategies ?? throw new ArgumentNullException(nameof(generationStrategies));
        _uniqueColors = uniqueColors ?? throw new ArgumentNullException(nameof(uniqueColors));
    }

    public void Generate(NonstopGameBlockFieldSettings fieldSettings)
    {
        ColorType[,,] colorTypes = new ColorType[fieldSettings.FieldSize.AmountLayers,
                                                 fieldSettings.FieldSize.AmountRows,
                                                 fieldSettings.FieldSize.AmountColumns];
        
        GenerateColorTypes(colorTypes, fieldSettings.FieldSize);

        fieldSettings.SetColorTypes(colorTypes);
    }

    private void GenerateColorTypes(ColorType[,,] colorTypes, FieldSize fieldSize)
    {
        GenerationStrategy generationStrategy = _generationStrategies[Random.Range(0, _generationStrategies.Count)];

        for (int row = 0; row < fieldSize.AmountRows; row++)
        {
            for (int layer = 0; layer < fieldSize.AmountLayers; layer++)
            {
                List<ColorType> colorType = generationStrategy.Generate(_uniqueColors, fieldSize.AmountColumns);

                for (int col = 0; col < fieldSize.AmountColumns; col++)
                {
                    colorTypes[layer, row, col] = colorType[col];
                }
            }
        }
    }
}