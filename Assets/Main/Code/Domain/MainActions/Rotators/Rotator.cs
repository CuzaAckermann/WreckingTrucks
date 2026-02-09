using System;
using UnityEngine;

public class Rotator : IRotator
{
    protected readonly IRotatable Rotatable;
    protected readonly float RotationSpeed;

    private Vector3 _target;

    public Rotator(IRotatable rotatable, float rotationSpeed)
    {
        Rotatable = rotatable ?? throw new ArgumentNullException(nameof(rotatable));
        RotationSpeed = rotationSpeed > 0 ? rotationSpeed : throw new ArgumentOutOfRangeException(nameof(rotationSpeed));
    }

    public event Action<ITargetAction> TargetChanged;
    public event Action<ITargetAction> TargetReached;
    public event Action<IDestroyable> DestroyedIDestroyable;

    public void Destroy()
    {
        DestroyedIDestroyable?.Invoke(this);
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
        TargetChanged?.Invoke(this);
    }

    public virtual void DoStep(float rotationStep)
    {
        if (Vector3.Angle(Rotatable.Forward, _target - Rotatable.Position) > rotationStep * RotationSpeed)
        {
            RotateStep(rotationStep);
        }
        else
        {
            FinishRotation();
        }
    }

    protected void OnTargetChanged()
    {
        TargetChanged?.Invoke(this);
    }

    protected void OnTargetReached()
    {
        TargetReached?.Invoke(this);
    }

    private Vector3 GetAxisOfRotation()
    {
        return Vector3.up;
    }

    private void RotateStep(float frameRotation)
    {
        float rotationAmount = frameRotation * GetSignRotationAmount();
        Quaternion rotation = Quaternion.AngleAxis(rotationAmount * RotationSpeed, GetAxisOfRotation());
        UpdateRotation(rotation);
    }

    private void FinishRotation()
    {
        Quaternion rotation = Quaternion.FromToRotation(Rotatable.Forward, _target - Rotatable.Position);
        UpdateRotation(rotation);
        TargetReached?.Invoke(this);
    }

    private void UpdateRotation(Quaternion rotation)
    {
        Rotatable.RotateForward(rotation);
    }

    private float GetSignRotationAmount()
    {
        Vector3 cross = Vector3.Cross(Rotatable.Forward, _target - Rotatable.Position);

        if (cross.y < 0)
        {
            return -1;
        }

        if (cross.y > 0)
        {
            return 1;
        }

        Logger.Log("PARALLEL");

        return 0;
    }
}