using System;
using UnityEngine;

public class Model : IModel
{
    private const int _angleToRight = -90;

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
        Right = Quaternion.Euler(0, _angleToRight, 0) * Forward;
        RotationChanged?.Invoke();
    }

    public void SetTargetRotation(Vector3 target)
    {
        Vector3 direction = target - Position;
        direction.y = 0;
        TargetRotation = direction;

        if (this is Gun)
        {
            Logger.Log("Prok2");
        }

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
            Quaternion rotation = Quaternion.AngleAxis(rotationAmount, Vector3.up);
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

    private void UpdatePosition(Vector3 nextPosition)
    {
        Position = nextPosition;
        PositionChanged?.Invoke();
    }

    private void UpdateRotation(Quaternion rotation)
    {
        Forward = rotation * Forward;
        Right = Quaternion.Euler(0, _angleToRight, 0) * Forward;
        RotationChanged?.Invoke();
    }

    private void CalculateDirectionToTarget()
    {
        DirectionToTarget = TargetPosition - Position;
        NormalizedDirection = DirectionToTarget.normalized;
        RotationChanged?.Invoke();
    }
}