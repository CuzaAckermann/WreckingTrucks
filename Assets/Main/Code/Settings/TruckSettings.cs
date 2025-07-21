using System;
using UnityEngine;

[Serializable]
public class TruckSettings
{
    [SerializeField] private Vector3 _gunPosition;
    [SerializeField] private Vector3 _trunkPosition;
    [SerializeField, Range(0.01f, 3)] private float _shotCooldown;

    public Vector3 GunPosition => _gunPosition;

    public Vector3 TrunkPosition => _trunkPosition;

    public float ShotCooldown => _shotCooldown;
}