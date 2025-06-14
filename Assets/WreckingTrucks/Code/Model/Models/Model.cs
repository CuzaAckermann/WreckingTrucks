using System;
using UnityEngine;

public abstract class Model
{
    public event Action PositionChanged;
    public event Action RotationChanged;
    public event Action<Model> TargetPositionChanged;
    public event Action<Model> Destroyed;

    public Vector3 Position { get; private set; }

    public Vector3 Forward { get; private set; }

    public Vector3 NormalizedDirection { get; private set; }

    public Vector3 DirectionToTarget { get; private set; }

    public Vector3 TargetPosition { get; private set; }

    public bool IsTurnClockwise => Vector3.Cross(Forward, NormalizedDirection).y < 0;

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
        UpdatePosition(Position + NormalizedDirection * distance);
        CalculateDirectionToTarget();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        TargetPositionChanged?.Invoke(this);
        CalculateDirectionToTarget();
    }

    public virtual void FinishMovement()
    {
        UpdatePosition(TargetPosition);
    }

    public void Rotate(Quaternion rotation)
    {
        Forward = rotation * Forward;
        RotationChanged?.Invoke();
    }

    public void FinishRotate()
    {
        Quaternion rotation = Quaternion.FromToRotation(Forward, NormalizedDirection);
        Rotate(rotation);
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
        NormalizedDirection = DirectionToTarget.normalized;
    }
}