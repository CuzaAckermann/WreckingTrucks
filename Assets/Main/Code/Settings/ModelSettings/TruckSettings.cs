using System;
using UnityEngine;

[Serializable]
public class TruckSettings
{
    [SerializeField, Range(0, 0.1f)] private float _shotCooldown;

    public float ShotCooldown => _shotCooldown;
}