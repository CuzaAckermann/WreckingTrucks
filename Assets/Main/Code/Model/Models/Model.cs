using System;
using UnityEngine;

public class Model
{
    protected readonly float SqrMovespeed;

    private readonly float _movespeed;
    private readonly float _rotatespeed;

    private Vector3 _axisOfRotation;

    public Model(float movespeed, float rotatespeed)
    {
        if (movespeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(movespeed));
        }

        if (rotatespeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rotatespeed));
        }

        _movespeed = movespeed;
        _rotatespeed = rotatespeed;

        SqrMovespeed = _movespeed * _movespeed;
    }

    public event Action PositionChanged;
    public event Action RotationChanged;

    public event Action<Model> FirstPositionDefined;

    public event Action<Model> TargetPositionChanged;
    public event Action<Model> TargetRotationChanged;

    public event Action<Model> TargetPositionReached;
    public event Action<Model> TargetRotationReached;

    public event Action<Model> Destroyed;

    public Vector3 Position { get; private set; }

    public Vector3 Forward { get; private set; }

    public Vector3 NormalizedDirection { get; private set; }

    public Vector3 DirectionToTarget { get; private set; }

    public Vector3 TargetPosition { get; protected set; }

    public Vector3 TargetForRotation { get; private set; }

    public ColorType ColorType { get; private set; }

    public virtual void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public virtual void SetColor(ColorType color)
    {
        ColorType = color;
    }

    public virtual void SetDirectionForward(Vector3 forward)
    {
        Forward = forward;
        RotationChanged?.Invoke();
    }

    public virtual void SetTargetRotation(Vector3 target)
    {
        TargetForRotation = GetTargetRotation(target);
        _axisOfRotation = GetAxisOfRotation();

        TargetRotationChanged?.Invoke(this);
    }

    public virtual void SetFirstPosition(Vector3 position)
    {
        UpdatePosition(position);
        FirstPositionDefined?.Invoke(this);
    }

    public virtual void SetPosition(Vector3 position)
    {
        UpdatePosition(position);
    }

    public virtual void SetTargetPosition(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        TargetPositionChanged?.Invoke(this);

        CalculateDirectionToTarget();
    }

    public virtual void Move(float frameMovement)
    {
        if (DirectionToTarget.sqrMagnitude > SqrMovespeed * frameMovement * frameMovement)
        {
            MoveStep(frameMovement);
        }
        else
        {
            FinishMovement();
        }
    }

    public virtual void Rotate(float frameRotation)
    {
        if (Vector3.Angle(Forward, DetermineTargetRotation() - Position) > frameRotation * _rotatespeed)
        {
            RotateStep(frameRotation);
        }
        else
        {
            FinishRotation();
        }
    }

    protected virtual Vector3 GetAxisOfRotation()
    {
        Vector3 cross = Vector3.Cross(Forward, DetermineTargetRotation() - Position);

        return cross.y < 0 ? -cross : cross;
    }

    protected virtual Vector3 GetTargetRotation(Vector3 target)
    {
        return target;
    }

    protected virtual Vector3 DetermineTargetRotation()
    {
        return TargetForRotation;
    }

    protected virtual void FinishMovement()
    {
        UpdatePosition(TargetPosition);
        TargetPositionReached?.Invoke(this);
    }

    private void MoveStep(float frameMovement)
    {
        Vector3 directionToNextPosition = _movespeed * frameMovement * NormalizedDirection;
        UpdatePosition(Position + directionToNextPosition);
        CalculateDirectionToTarget();
    }

    private void UpdatePosition(Vector3 nextPosition)
    {
        Position = nextPosition;
        PositionChanged?.Invoke();
    }

    private void RotateStep(float frameRotation)
    {
        float rotationAmount = Vector3.Cross(Forward, DetermineTargetRotation() - Position).y < 0 ? -frameRotation : frameRotation;
        Quaternion rotation = Quaternion.AngleAxis(rotationAmount * _rotatespeed, GetAxisOfRotation());
        UpdateRotation(rotation);
    }

    private void FinishRotation()
    {
        Quaternion rotation = Quaternion.FromToRotation(Forward, DetermineTargetRotation() - Position);
        UpdateRotation(rotation);
        TargetRotationReached?.Invoke(this);
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
    }
}