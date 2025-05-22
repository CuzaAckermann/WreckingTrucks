using System;
using UnityEngine;

public abstract class Block
{
    private Vector3 _targetPosition;
    private Vector3 _normalizedDirection;

    public event Action PositionChanged;
    public event Action<Block> Destroyed;

    public Vector3 Position { get; private set; }

    public Vector3 DirectionToTarget {  get; private set; }

    public abstract void Accept(IBlockVisitor blockVisitor);

    public void SetStartPosition(Vector3 position)
    {
        UpdatePosition(position);
    }

    public void Move(float distance)
    {
        UpdatePosition(Position + _normalizedDirection * distance);
        CalculateDirectionToTarget();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        CalculateDirectionToTarget();
    }

    public void FinishMovement()
    {
        UpdatePosition(_targetPosition);
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    private void UpdatePosition(Vector3 nextPosition)
    {
        Position = nextPosition;
        PositionChanged?.Invoke();
    }

    private void CalculateDirectionToTarget()
    {
        DirectionToTarget = _targetPosition - Position;
        _normalizedDirection = DirectionToTarget.normalized;
    }
}