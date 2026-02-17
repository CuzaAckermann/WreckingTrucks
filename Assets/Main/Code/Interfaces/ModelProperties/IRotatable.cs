using System;
using UnityEngine;

public interface IRotatable
{
    public event Action RotationChanged;

    public Vector3 Position { get; }

    public Vector3 Forward { get; }

    public void SetForward(Vector3 forward);

    public void RotateForward(Quaternion rotationStep);
}