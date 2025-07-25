using System;
using System.Linq;
using Random = UnityEngine.Random;

public class ModelTypeGenerator<M> where M : Model
{
    private readonly ModelProbabilitySettings<M> _modelProbabilitySettings;
    private readonly float _minAmountProbabilityReduction;
    private readonly float _maxAmountProbabilityReduction;

    private float _amountProbabilityReduction;

    public ModelTypeGenerator(ModelProbabilitySettings<M> modelProbabilitySettings,
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

    public Type Generate()
    {
        float randomValue = Random.value;
        float cumulative = 0f;
        Type randomModelType = null;

        foreach (var probability in _modelProbabilitySettings.Probabilities)
        {
            cumulative += probability.Value;

            if (randomValue <= cumulative)
            {
                randomModelType = probability.Key;
                break;
            }
        }

        if (randomModelType == null)
        {
            randomModelType = _modelProbabilitySettings.Probabilities.Keys.Last();
        }

        _amountProbabilityReduction = Random.Range(_minAmountProbabilityReduction, _maxAmountProbabilityReduction);
        _modelProbabilitySettings.ChangeProbabilities(randomModelType, _amountProbabilityReduction);

        return randomModelType;
    }
}