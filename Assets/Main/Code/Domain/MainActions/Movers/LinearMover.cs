using System;
using UnityEngine;

public class LinearMover : IMover
{
    private readonly IMovable _movable;
    
    private protected readonly float SqrMovespeed;

    private readonly float _movespeed;

    public LinearMover(IMovable movable, float movespeed)
    {
        _movable = movable ?? throw new ArgumentNullException(nameof(movable));
        _movespeed = movespeed > 0 ? movespeed : throw new ArgumentOutOfRangeException(nameof(movespeed));

        SqrMovespeed = _movespeed * _movespeed;
    }

    public event Action TargetPositionChanged;
    public event Action TargetPositionReached;

    public Vector3 DirectionToTarget { get; private set; }

    public Vector3 NormalizedDirection { get; private set; }

    public Vector3 TargetPosition { get; protected set; }

    public void SetPosition(Vector3 position)
    {
        _movable.SetPosition(position);
    }

    public void Move(float movementStep)
    {
        if (DirectionToTarget.sqrMagnitude > SqrMovespeed * movementStep * movementStep)
        {
            MoveStep(movementStep);
        }
        else
        {
            FinishMovement();
        }
    }

    public virtual void SetTargetPosition(Vector3 targetPosition)
    {
        TargetPosition = targetPosition;
        CalculateDirectionToTarget();

        TargetPositionChanged?.Invoke();
    }

    public void FinishMovement()
    {
        _movable.SetPosition(TargetPosition);

        TargetPositionReached?.Invoke();
    }

    private void MoveStep(float movementStep)
    {
        _movable.ShiftPosition(_movespeed * movementStep * NormalizedDirection);
        CalculateDirectionToTarget();
    }

    private void CalculateDirectionToTarget()
    {
        DirectionToTarget = TargetPosition - _movable.Position;
        NormalizedDirection = DirectionToTarget.normalized;
    }
}