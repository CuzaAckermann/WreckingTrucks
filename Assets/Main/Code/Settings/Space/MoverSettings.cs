using System;
using UnityEngine;

[Serializable]
public class MoverSettings
{
    [SerializeField] private int _capacityMoveables;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _minSqrDistanceToTargetPosition;

    // убери это
    public MoverSettings(int capacityMoveables,
                         float movementSpeed,
                         float min)
    {
        _capacityMoveables = capacityMoveables;
        _movementSpeed = movementSpeed;
        _minSqrDistanceToTargetPosition = min;
    }
    // убери это

    public int CapacityMoveables => _capacityMoveables;

    public float MovementSpeed => _movementSpeed;

    public float MinSqrDistanceToTargetPosition => _minSqrDistanceToTargetPosition;
}