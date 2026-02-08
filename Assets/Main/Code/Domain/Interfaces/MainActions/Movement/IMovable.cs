using System;
using UnityEngine;

public interface IMovable
{
    public event Action PositionChanged;

    public Vector3 Position { get; }

    public void SetPosition(Vector3 position);

    public void ShiftPosition(Vector3 movementStep);
}