using System;
using UnityEngine;

[Serializable]
public class SmoothValueFollowerSettings
{
    [SerializeField] private float _initialValue;
    [SerializeField] private float _speed;

    public float InitialValue => _initialValue;

    public float Speed => _speed;
}