using System;
using UnityEngine;

[Serializable]
public class AIClickerSettings
{
    [SerializeField, Range(1, 10)] private float _startDelay;
    [SerializeField, Range(0.1f, 2)] private float _minFrequency;
    [SerializeField, Range(2, 5)] private float _maxFrequency;

    public float StartDelay => _startDelay;

    public float MinFrequency => _minFrequency;

    public float MaxFrequency => _maxFrequency;
}