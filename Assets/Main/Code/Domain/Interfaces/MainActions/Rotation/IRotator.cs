using System;
using UnityEngine;

public interface IRotator
{
    public event Action TargetRotationChanged;
    public event Action TargetRotationReached;

    public Vector3 TargetForRotation { get; }

    public void SetForward(Vector3 forward);

    public void SetTargetRotation(Vector3 target);

    public void Rotate(float rotationStep);
}