using System;
using UnityEngine;

[Serializable]
public class StrategyFillingSettings
{
    [SerializeField] private float _frequency;
    [SerializeField] private bool _isUsing;

    public float Frequency => _frequency;

    public bool IsUsing => _isUsing;
}