using System;
using UnityEngine;

[Serializable]
public class ValueButtonSettings
{
    [SerializeField] private KeyCode _button;
    [SerializeField] private float _value;

    public KeyCode Button => _button;

    public float Value => _value;
}