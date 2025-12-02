using System;
using UnityEngine;

[Serializable]
public class GunSettings : ModelSettings
{
    [SerializeField, Range(0, 0.1f)] private float _shotCooldown;
    [SerializeField, Min(1)] private int _capacity;

    public float ShotCooldown => _shotCooldown;

    public int Capacity => _capacity;
}