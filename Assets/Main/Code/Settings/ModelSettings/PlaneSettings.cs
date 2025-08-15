using System;
using UnityEngine;

[Serializable]
public class PlaneSettings
{
    [SerializeField] private Vector3 _gunPosition;
    [SerializeField] private Vector3 _trunkPosition;
    [SerializeField, Range(0.01f, 3)] private float _shotCooldown;
    [SerializeField, Min(1)] private int _amountDestroyedRows;

    public Vector3 GunPosition => _gunPosition;

    public Vector3 TrunkPosition => _trunkPosition;

    public float ShotCooldown => _shotCooldown;

    public int AmountDestroyedRows => _amountDestroyedRows;
}