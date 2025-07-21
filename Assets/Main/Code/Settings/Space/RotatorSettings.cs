using System;
using UnityEngine;

[Serializable]
public class RotatorSettings
{
    [SerializeField, Min(1)] private int _capacityRotatables;
    [SerializeField, Min(1)] private float _rotationSpeed;
    [SerializeField, Min(0.01f)] private float _minAngleToFinish;

    public int CapacityRotatables => _capacityRotatables;

    public float RotationSpeed => _rotationSpeed;

    public float MinAngleToFinish => _minAngleToFinish;
}