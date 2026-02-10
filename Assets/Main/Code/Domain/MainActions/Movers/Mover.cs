using System;
using UnityEngine;

public class Mover : IMover
{
    private readonly IMovable _movable;

    private readonly float _movespeed;
    private readonly float _sqrMovespeed;

    private Vector3 _directionToTarget;
    private Vector3 _normalizedDirection;
    private Vector3 _target;

    public Mover(IMovable movable, float movespeed)
    {
        _movable = movable ?? throw new ArgumentNullException(nameof(movable));
        _movespeed = movespeed > 0 ? movespeed : throw new ArgumentOutOfRangeException(nameof(movespeed));

        _sqrMovespeed = _movespeed * _movespeed;
    }

    public event Action<ITargetAction> TargetChanged;
    public event Action<ITargetAction> TargetReached;
    public event Action<IDestroyable> Destroyed;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void DoStep(float movementStep)
    {
        if (_directionToTarget.sqrMagnitude > _sqrMovespeed * movementStep * movementStep)
        {
            MoveStep(movementStep);
        }
        else
        {
            FinishMovement();
        }
    }

    public virtual void SetTarget(Vector3 targetPosition)
    {
        _target = targetPosition;
        CalculateDirectionToTarget();

        TargetChanged?.Invoke(this);
    }

    private void FinishMovement()
    {
        _movable.SetPosition(_target);

        TargetReached?.Invoke(this);
    }

    private void MoveStep(float movementStep)
    {
        _movable.ShiftPosition(_movespeed * movementStep * _normalizedDirection);
        CalculateDirectionToTarget();
    }

    private void CalculateDirectionToTarget()
    {
        _directionToTarget = _target - _movable.Position;
        _normalizedDirection = _directionToTarget.normalized;
    }
}