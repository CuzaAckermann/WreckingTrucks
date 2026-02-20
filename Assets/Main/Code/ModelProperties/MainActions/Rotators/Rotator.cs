using System;
using UnityEngine;

public class Rotator : IRotator
{
    protected readonly IRotatable Rotatable;
    protected readonly float RotationSpeed;

    private Vector3 _target;

    public Rotator(IRotatable rotatable, float rotationSpeed)
    {
        Validator.ValidateNotNull(rotatable);
        Validator.ValidateMin(rotationSpeed, 0, true);

        Rotatable = rotatable;
        RotationSpeed = rotationSpeed;
    }

    public event Action<ITickable> Activated;
    public event Action<ITickable> Deactivated;

    //public event Action<ITargetAction> TargetChanged;
    //public event Action<ITargetAction> TargetReached;
    public event Action<IDestroyable> Destroyed;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;

        Activated?.Invoke(this);
        //TargetChanged?.Invoke(this);
    }

    public virtual void Tick(float rotationStep)
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

    protected void OnActivated()
    {
        Activated?.Invoke(this);

        //TargetChanged?.Invoke(this);
    }

    protected void OnDeactivated()
    {
        Deactivated?.Invoke(this);

        //TargetReached?.Invoke(this);
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
        // Поворот должен быть исходя из оси поворота GetAxisOfRotation, а не доворачивания до цели используя FromToRotation
        Quaternion rotation = Quaternion.FromToRotation(Rotatable.Forward, _target - Rotatable.Position);
        UpdateRotation(rotation);

        Deactivated?.Invoke(this);
        //TargetReached?.Invoke(this);
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