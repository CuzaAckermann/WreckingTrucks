using System;
using UnityEngine;

public class TargetFollower : IMover
{
    private readonly IMovable _movable;

    private readonly float _movespeed;
    private readonly float _minDistanceToTarget;

    private readonly float _sqrMovespeed;
    private readonly float _sqrMinDistanceToTarget;

    private Vector3 _directionToTarget;
    private Vector3 _normalizedDirection;


    private Target<Placeable> _target;

    public TargetFollower(IMovable movable, float movespeed, float minDistanceToTarget)
    {
        Validator.ValidateNotNull(movable);
        Validator.ValidateMin(movespeed, 0, true);
        Validator.ValidateMin(minDistanceToTarget, 0, false);

        _movable = movable;
        _movespeed = movespeed;
        _minDistanceToTarget = minDistanceToTarget;

        _sqrMovespeed = _movespeed * _movespeed;
        _sqrMinDistanceToTarget = minDistanceToTarget * minDistanceToTarget;
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
        if ((_target.Position - _movable.Position).sqrMagnitude - _sqrMinDistanceToTarget > _sqrMovespeed * movementStep * movementStep)
        {
            MoveStep(movementStep);
        }
        else
        {
            FinishMovement();
        }
    }

    public void SetTarget(Target<Placeable> target)
    {
        Validator.ValidateNotNull(target);

        _target = target;

        Activated?.Invoke(this);
    }

    public void SetTarget(Vector3 target)
    {
        Activated?.Invoke(this);
    }

    private void FinishMovement()
    {
        Vector3 directionToThisPosition = _movable.Position - _target.Position;

        Vector3 finishPosition = directionToThisPosition.normalized * _minDistanceToTarget;

        _movable.SetPosition(finishPosition);

        Deactivated?.Invoke(this);
        //TargetReached?.Invoke(this);
    }

    private void MoveStep(float movementStep)
    {
        _movable.ShiftPosition(_movespeed * movementStep * _normalizedDirection);
        CalculateDirectionToTarget();
    }

    private void CalculateDirectionToTarget()
    {
        _directionToTarget = _target.Position - _movable.Position;
        _normalizedDirection = _directionToTarget.normalized;
    }
}