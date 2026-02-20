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
        Validator.ValidateNotNull(movable);
        Validator.ValidateMin(movespeed, 0, true);

        _movable = movable;
        _movespeed = movespeed;

        _sqrMovespeed = _movespeed * _movespeed;
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

    public void Tick(float movementStep)
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

        Activated?.Invoke(this);
    }

    private void FinishMovement()
    {
        _movable.SetPosition(_target);

        Deactivated?.Invoke(this);
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