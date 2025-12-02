using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public abstract class ModelGenerator<M> where M : Model
{
    private readonly ModelFactory<M> _modelFactory;
    private readonly ModelProbabilitySettings _modelProbabilitySettings;
    private readonly float _minAmountProbabilityReduction;
    private readonly float _maxAmountProbabilityReduction;

    private float _amountProbabilityReduction;

    public ModelGenerator(ModelFactory<M> modelFactory,
                          ModelProbabilitySettings modelProbabilitySettings,
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

        _modelFactory = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));
        _modelProbabilitySettings = modelProbabilitySettings ?? throw new ArgumentNullException(nameof(modelProbabilitySettings));

        _minAmountProbabilityReduction = minAmountProbabilityReduction;
        _maxAmountProbabilityReduction = maxAmountProbabilityReduction;
    }

    public M Generate()
    {
        M model = _modelFactory.Create();
        ColorType colorType = GenerateEvenly();
        model.SetColor(colorType);

        return model;
    }

    public void SetColorTypes(IReadOnlyList<ColorType> colorTypes)
    {
        _modelProbabilitySettings.SetColorTypes(colorTypes);
    }

    private ColorType GenerateEvenly()
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

        _amountProbabilityReduction = Random.Range(_minAmountProbabilityReduction, _maxAmountProbabilityReduction);
        _modelProbabilitySettings.ChangeProbabilities(randomModelType, _amountProbabilityReduction);

        return randomModelType;
    }
}