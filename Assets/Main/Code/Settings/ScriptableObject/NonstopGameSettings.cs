using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NonstopGameSettings
{
    [Header("Block Field")]
    [SerializeField] private FieldSize _blockFieldSize;
    [SerializeField] private List<ColorType> _generatedColorTypes;
    [SerializeField] private float _frequency;

    [Header("Cartridge Box Field")]
    [SerializeField, Min(1)] private int _amountCartrigeBoxes;

    public FieldSize BlockFieldSize => _blockFieldSize;

    public IReadOnlyList<ColorType> GeneratedColorTypes => _generatedColorTypes;

    public float Frequency => _frequency;

    public int AmountCartrigeBoxes => _amountCartrigeBoxes;
}