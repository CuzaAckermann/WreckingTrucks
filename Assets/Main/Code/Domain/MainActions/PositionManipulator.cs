using System;
using UnityEngine;

public class PositionManipulator : IMovable, IRotatable
{
    public event Action PositionChanged;

    public event Action RotationChanged;

    public Vector3 Position { get; private set; }

    public Vector3 Forward { get; private set; }

    public void SetPosition(Vector3 position)
    {
        UpdatePosition(position);
    }

    public void ShiftPosition(Vector3 movementStep)
    {
        UpdatePosition(Position + movementStep);
    }

    public void SetForward(Vector3 forward)
    {
        UpdateRotation(forward);
    }

    public void RotateForward(Quaternion rotation)
    {
        UpdateRotation(rotation * Forward);
    }

    private void UpdatePosition(Vector3 nextPosition)
    {
        Position = nextPosition;
        PositionChanged?.Invoke();
    }

    private void UpdateRotation(Vector3 forward)
    {
        Forward = forward;
        RotationChanged?.Invoke();
    }
}