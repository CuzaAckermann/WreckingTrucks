using System;
using UnityEngine;

public interface IMover
{
    public event Action TargetPositionChanged;
    public event Action TargetPositionReached;

    public Vector3 DirectionToTarget { get; }

    public Vector3 NormalizedDirection { get; }

    public Vector3 TargetPosition { get; }

    public void SetPosition(Vector3 position);

    public void Move(float movementStep);

    public void SetTargetPosition(Vector3 targetPosition);

    public void FinishMovement();
}