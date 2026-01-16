using System;
using UnityEngine;

[Serializable]
public class StrategyFillingSettings
{
    [SerializeField, Min(0.001f)] private float _frequency;
    [SerializeField] private bool _isUsing;
    [SerializeField, Min(1)] private int _spawnDistance;

    public float Frequency => _frequency;

    public bool IsUsing => _isUsing;

    public int SpawnDistance => _spawnDistance;
}