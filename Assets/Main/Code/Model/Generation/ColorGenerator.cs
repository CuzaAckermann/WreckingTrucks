using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class ColorGenerator
{
    private readonly ModelProbabilitySettings _modelProbabilitySettings;
    private readonly float _minAmountProbabilityReduction;
    private readonly float _maxAmountProbabilityReduction;

    public ColorGenerator(ModelProbabilitySettings modelProbabilitySettings,
                          float minAmountProbabilityReduction,
                          float maxAmountProbabilityReduction)
    {
        if (minAmountProbabilityReduction <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minAmountProbabilityReduction));
        }

        if (maxAmountProbabilityReduction <= 0)
        {
            throw new ArgumentNullException(nameof(maxAmountProbabilityReduction));
        }

        if (maxAmountProbabilityReduction <= minAmountProbabilityReduction)
        {
            throw new ArgumentOutOfRangeException(nameof(maxAmountProbabilityReduction));
        }

        _modelProbabilitySettings = modelProbabilitySettings ?? throw new ArgumentNullException(nameof(modelProbabilitySettings));

        _minAmountProbabilityReduction = minAmountProbabilityReduction;
        _maxAmountProbabilityReduction = maxAmountProbabilityReduction;
    }

    public void SetColorTypes(IReadOnlyList<ColorType> colorTypes)
    {
        _modelProbabilitySettings.SetColorTypes(colorTypes);
    }

    public IReadOnlyList<ColorType> GetGeneratedColors()
    {
        return _modelProbabilitySettings.GetColors();
    }

    public ColorType GenerateEvenly()
    {
        float randomValue = Random.value;
        float cumulative = 0f;
        ColorType randomModelType = ColorType.Unknown;

        foreach (var probability in _modelProbabilitySettings.Probabilities)
        {
            cumulative += probability.Value;

            if (randomValue <= cumulative)
            {
                randomModelType = probability.Key;
                break;
            }
        }

        if (randomModelType == ColorType.Unknown)
        {
            randomModelType = _modelProbabilitySettings.Probabilities.Keys.Last();
        }

        float amountProbabilityReduction = Random.Range(_minAmountProbabilityReduction, _maxAmountProbabilityReduction);
        _modelProbabilitySettings.ChangeProbabilities(randomModelType, amountProbabilityReduction);

        return randomModelType;
    }
}
