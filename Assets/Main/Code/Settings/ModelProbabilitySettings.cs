using System;
using System.Collections.Generic;

public class ModelProbabilitySettings<M> where M : Model
{
    private readonly Dictionary<Type, float> _probabilities;
    private readonly float _defaultProbability;

    public ModelProbabilitySettings(List<Type> modelTypes)
    {
        if (modelTypes == null)
        {
            throw new ArgumentNullException(nameof(modelTypes));
        }

        if (modelTypes.Count <= 1)
        {
            throw new InvalidOperationException($"Not enough types");
        }

        _defaultProbability = 1f / modelTypes.Count;

        _probabilities = new Dictionary<Type, float>();

        FillProbabilities(modelTypes);
    }

    public IReadOnlyDictionary<Type, float> Probabilities => _probabilities;

    public void ResetProbabilities()
    {
        foreach (Type modelType in _probabilities.Keys)
        {
            _probabilities[modelType] = _defaultProbability;
        }
    }

    public void ChangeProbabilities(Type decreasedType, float amountDecreaseProbability)
    {
        if (_probabilities.ContainsKey(decreasedType) == false)
        {
            throw new InvalidOperationException($"Type {decreasedType} not found in probabilities");
        }

        if (amountDecreaseProbability <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountDecreaseProbability));
        }

        float amountIncreaseProbability = (_probabilities.Count == 2) ? amountDecreaseProbability : amountDecreaseProbability / (_probabilities.Count - 1);

        List<Type> keys = new List<Type>(_probabilities.Keys);

        foreach (Type key in keys)
        {
            if (key == decreasedType)
            {
                _probabilities[key] -= amountDecreaseProbability;
            }
            else
            {
                _probabilities[key] += amountIncreaseProbability;
            }
        }

        if (_probabilities[decreasedType] < 0)
        {
            _probabilities[decreasedType] = 0;
        }

        NormalizeProbabilities();
    }

    public void NormalizeProbabilities()
    {
        float sum = 0f;

        foreach (float probability in _probabilities.Values)
        {
            sum += probability;
        }

        List<Type> blockTypes = new List<Type>(_probabilities.Keys);

        foreach (Type blockType in blockTypes)
        {
            _probabilities[blockType] /= sum;
        }
    }

    private void FillProbabilities(List<Type> types)
    {
        for (int i = 0; i < types.Count; i++)
        {
            _probabilities[types[i]] = _defaultProbability;
        }
    }
}