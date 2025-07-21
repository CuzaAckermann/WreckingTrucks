using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TruckFieldSettings : FieldSettings
{
    [SerializeField] private List<ColorType> _types;
    [SerializeField] private float _amountProbabilityReduction;

    public IReadOnlyList<ColorType> Types => _types;

    public float AmountProbabilityReduction => _amountProbabilityReduction;
}