using System;
using UnityEngine;

[Serializable]
public class Block
{
    private Vector3 _normalizedDirection;
    private Vector3 _position;
    private Vector3 _targetPosition;

    public event Action PositionChanged;
    public event Action<Block> Destroyed;

    public Vector3 Position => _position;

    public float SqrDistanceToTarget => (_targetPosition - _position).sqrMagnitude;

    public void SetStartPosition(Vector3 position)
    {
        _position = position;
        PositionChanged?.Invoke();
    }

    public void Move(float distance)
    {
        _position += _normalizedDirection * distance;
        PositionChanged?.Invoke();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
        _normalizedDirection = (_targetPosition - _position).normalized;
    }

    public void FinishMovement()
    {
        _position = _targetPosition;
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }
}