using System;
using UnityEngine;

[Serializable]
public class DeltaTimeFactorSettings
{
    [SerializeField] private float _initial = 1;
    [SerializeField] private float _min = 0;
    [SerializeField] private float _max = 100;

    public float Initial => _initial;

    public float Min => _min;

    public float Max => _max;
}