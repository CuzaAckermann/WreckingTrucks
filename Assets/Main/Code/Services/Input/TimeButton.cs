using System;
using UnityEngine;

[Serializable]
public class TimeButton : IInputButton
{
    [SerializeField] private KeyCode _button;
    [SerializeField] private float _timeCoefficient;

    public event Action Pressed;

    public KeyCode Button => _button;

    public float TimeCoefficient => _timeCoefficient;

    public void Update()
    {
        Pressed?.Invoke();
    }
}