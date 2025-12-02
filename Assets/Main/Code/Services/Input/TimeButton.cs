using System;
using UnityEngine;

[Serializable]
public class TimeButton
{
    [SerializeField] private KeyCode _button;
    [SerializeField] private float _timeCoefficient;

    public KeyCode Button => _button;

    public float TimeCoefficient => _timeCoefficient;
}