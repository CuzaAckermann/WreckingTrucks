using System;
using System.Linq;
using Random = UnityEngine.Random;

public class ModelTypeGenerator<M> where M : Model
{
    private readonly ModelProbabilitySettings<M> _modelProbabilitySettings;
    private readonly float _amountProbabilityReduction;

    public ModelTypeGenerator(ModelProbabilitySettings<M> modelProbabilitySettings,
                              float amountProbabilityReduction)
    {
        if (amountProbabilityReduction <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountProbabilityReduction));
        }

        _modelProbabilitySettings = modelProbabilitySettings ?? throw new ArgumentNullException(nameof(modelProbabilitySettings));
        _amountProbabilityReduction = amountProbabilityReduction;
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

        _modelProbabilitySettings.ChangeProbabilities(randomModelType, _amountProbabilityReduction);

        return randomModelType;
    }
}