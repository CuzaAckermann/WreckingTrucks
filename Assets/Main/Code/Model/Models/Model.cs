using System;
using UnityEngine;

public class Model : IModel
{
    private Vector3 _axisOfRotation;

    public event Action PositionChanged;
    public event Action RotationChanged;

    public event Action<Model> TargetRotationChanged;

    public event Action<Model> TargetPositionReached;
    public event Action<Model> TargetRotationReached;

    public event Action<Model> Destroyed;
    public event Action<IModel> InterfaceDestroyed;

    public Vector3 Position { get; private set; }

    public Vector3 Forward { get; private set; }

    public Vector3 Right { get; private set; }

    public Vector3 NormalizedDirection { get; private set; }

    public Vector3 DirectionToTarget { get; private set; }

    public Vector3 TargetPosition { get; private set; }

    public Vector3 TargetRotation { get; private set; }

    public virtual void Destroy()
    {
        Destroyed?.Invoke(this);
        InterfaceDestroyed?.Invoke(this);
    }

    public virtual void SetDirectionForward(Vector3 forward)
    {
        Forward = forward;
        RotationChanged?.Invoke();
    }

    public void SetTargetRotation(Vector3 target)
    {
        TargetRotation = GetTargetRotation(target);
        _axisOfRotation = GetAxisOfRotation();
        TargetRotationChanged?.Invoke(this);
    }

    public virtual void SetPosition(Vector3 position)
    {
        UpdatePosition(position);
    }

    public virtual void SetTargetPosition(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        CalculateDirectionToTarget();
    }

    public virtual void Move(float frameMovement)
    {
        if ((TargetPosition - Position).sqrMagnitude > frameMovement * frameMovement)
        {
            UpdatePosition(Position + NormalizedDirection * frameMovement);
            CalculateDirectionToTarget();
        }
        else
        {
            FinishMovement();
        }
    }

    public virtual void Rotate(float frameRotation)
    {
        if (Vector3.Angle(Forward, TargetRotation) > frameRotation)
        {
            float rotationAmount = Vector3.Cross(Forward, TargetRotation).y < 0 ? -frameRotation : frameRotation;
            Quaternion rotation = Quaternion.AngleAxis(rotationAmount, _axisOfRotation);
            UpdateRotation(rotation);
        }
        else
        {
            FinishRotate();
        }
    }

    protected virtual void FinishMovement()
    {
        UpdatePosition(TargetPosition);
        TargetPositionReached?.Invoke(this);
    }

    protected virtual void FinishRotate()
    {
        Quaternion rotation = Quaternion.FromToRotation(Forward, TargetRotation);
        UpdateRotation(rotation);
        TargetRotationReached?.Invoke(this);
    }

    protected virtual Vector3 GetAxisOfRotation()
    {
        Vector3 cross = Vector3.Cross(Forward, TargetRotation);

        return cross.y < 0 ? -cross : cross;
    }

    protected virtual Vector3 GetTargetRotation(Vector3 target)
    {
        return target - Position;
    }

    private void UpdatePosition(Vector3 nextPosition)
    {
        Position = nextPosition;
        PositionChanged?.Invoke();
    }

    private void UpdateRotation(Quaternion rotation)
    {
        Forward = rotation * Forward;
        RotationChanged?.Invoke();
    }

    private void CalculateDirectionToTarget()
    {
        DirectionToTarget = TargetPosition - Position;
        NormalizedDirection = DirectionToTarget.normalized;
        RotationChanged?.Invoke();
    }
}