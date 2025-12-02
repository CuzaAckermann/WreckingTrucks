using System;
using UnityEngine;

[Serializable]
public class ModelSettings
{
    [SerializeField] private float _movespeed;
    [SerializeField] private float _rotatespeed;

    public float Movespeed => _movespeed;

    public float Rotatespeed => _rotatespeed;
}