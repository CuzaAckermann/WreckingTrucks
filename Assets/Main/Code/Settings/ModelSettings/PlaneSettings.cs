using System;
using UnityEngine;

[Serializable]
public class PlaneSettings
{
    [SerializeField, Range(0.0001f, 0.1f)] private float _shotCooldown;
    [SerializeField, Min(1)] private int _amountDestroyedRows;

    public float ShotCooldown => _shotCooldown;

    public int AmountDestroyedRows => _amountDestroyedRows;
}