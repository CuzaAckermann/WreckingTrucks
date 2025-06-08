using System;
using UnityEngine;

public abstract class Model
{
    private Vector3 _normalizedDirection;

    public event Action PositionChanged;
    public event Action<Model> Destroyed;

    public Vector3 Position { get; private set; }

    public Vector3 Forward { get; private set; }

    public Vector3 DirectionToTarget { get; private set; }

    public Vector3 TargetPosition { get; private set; }

    public void SetDirectionForward(Vector3 forward)
    {
        Forward = forward;
    }

    public void SetStartPosition(Vector3 position)
    {
        UpdatePosition(position);
    }

    public void Move(float distance)
    {
        UpdatePosition(Position + _normalizedDirection * distance);
        CalculateDirectionToTarget();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        CalculateDirectionToTarget();
    }

    public void FinishMovement()
    {
        UpdatePosition(TargetPosition);
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    private void UpdatePosition(Vector3 nextPosition)
    {
        Position = nextPosition;
        PositionChanged?.Invoke();
    }

    private void CalculateDirectionToTarget()
    {
        DirectionToTarget = TargetPosition - Position;
        _normalizedDirection = DirectionToTarget.normalized;
    }
}