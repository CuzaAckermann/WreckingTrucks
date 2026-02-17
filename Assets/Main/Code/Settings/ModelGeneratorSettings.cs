using System;
using UnityEngine;

[Serializable]
public class ModelGeneratorSettings
{
    [SerializeField] private float _minAmountProbabilityReduction;
    [SerializeField] private float _maxAmountProbabilityReduction;

    public float MinAmountProbabilityReduction => _minAmountProbabilityReduction;

    public float MaxAmountProbabilityReduction => _maxAmountProbabilityReduction;
}