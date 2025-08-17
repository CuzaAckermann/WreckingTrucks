using System;
using UnityEngine;

[Serializable]
public class PlaneSettings
{
    [SerializeField, Range(0.01f, 3)] private float _shotCooldown;
    [SerializeField, Min(1)] private int _amountDestroyedRows;

    public float ShotCooldown => _shotCooldown;

    public int AmountDestroyedRows => _amountDestroyedRows;
}