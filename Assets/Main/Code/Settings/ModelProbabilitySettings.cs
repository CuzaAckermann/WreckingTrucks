using System;
using System.Collections.Generic;
using System.Linq;

public class ModelProbabilitySettings
{
    private readonly Dictionary<ColorType, float> _probabilities;

    private float _defaultProbability;

    public ModelProbabilitySettings()
    {
        _probabilities = new Dictionary<ColorType, float>();
    }

    public IReadOnlyDictionary<ColorType, float> Probabilities => _probabilities;

    public void SetColorTypes(IReadOnlyList<ColorType> colorTypes)
    {
        if (colorTypes == null)
        {
            throw new ArgumentNullException(nameof(colorTypes));
        }

        if (colorTypes.Count <= 1)
        {
            throw new InvalidOperationException($"Not enough types");
        }

        FillProbabilities(colorTypes);
    }

    public void ResetProbabilities()
    {
        FillProbabilities(_probabilities.Keys.ToList());
    }

    public void ChangeProbabilities(ColorType decreasedType, float amountDecreaseProbability)
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

        List<ColorType> keys = new List<ColorType>(_probabilities.Keys);

        foreach (ColorType key in keys)
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

    private void NormalizeProbabilities()
    {
        float sum = 0f;

        foreach (float probability in _probabilities.Values)
        {
            sum += probability;
        }

        List<ColorType> modelTypes = new List<ColorType>(_probabilities.Keys);

        foreach (ColorType modelType in modelTypes)
        {
            _probabilities[modelType] /= sum;
        }
    }

    private void FillProbabilities(IReadOnlyList<ColorType> types)
    {
        _defaultProbability = 1f / types.Count;

        for (int i = 0; i < types.Count; i++)
        {
            _probabilities[types[i]] = _defaultProbability;
        }
    }
}