using System;
using UnityEngine;

public class LinearRotator : IRotator
{
    private readonly IRotatable _rotatable;
    private readonly float _rotatespeed;

    public LinearRotator(IRotatable rotatable, float rotatespeed)
    {
        _rotatable = rotatable ?? throw new ArgumentNullException(nameof(rotatable));
        _rotatespeed = rotatespeed > 0 ? rotatespeed : throw new ArgumentOutOfRangeException(nameof(rotatespeed));
    }

    public event Action TargetRotationChanged;
    public event Action TargetRotationReached;

    public Vector3 TargetForRotation { get; private set; }

    public void SetForward(Vector3 forward)
    {
        _rotatable.SetForward(forward);
    }

    public void SetTargetRotation(Vector3 target)
    {
        TargetForRotation = target;
        TargetRotationChanged?.Invoke();
    }

    public void Rotate(float rotationStep)
    {
        if (Vector3.Angle(_rotatable.Forward, TargetForRotation - _rotatable.Position) > rotationStep * _rotatespeed)
        {
            RotateStep(rotationStep);
        }
        else
        {
            FinishRotation();
        }
    }

    private Vector3 GetAxisOfRotation()
    {
        return Vector3.up;
    }

    private void RotateStep(float frameRotation)
    {
        float rotationAmount = Vector3.Cross(_rotatable.Forward, TargetForRotation - _rotatable.Position).y < 0 ? -frameRotation : frameRotation;
        Quaternion rotation = Quaternion.AngleAxis(rotationAmount * _rotatespeed, GetAxisOfRotation());
        UpdateRotation(rotation);
    }

    private void FinishRotation()
    {
        Quaternion rotation = Quaternion.FromToRotation(_rotatable.Forward, TargetForRotation - _rotatable.Position);
        UpdateRotation(rotation);
        TargetRotationReached?.Invoke();
    }

    private void UpdateRotation(Quaternion rotation)
    {
        _rotatable.RotateForward(rotation);
    }
}