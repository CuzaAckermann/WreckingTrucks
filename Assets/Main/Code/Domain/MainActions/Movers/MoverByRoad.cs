using System;
using UnityEngine;

public class MoverByRoad// : IMover
{
    private readonly Road _road;
    private int _currentPoint;

    private Vector3 _targetPosition;

    public event Action<ITargetAction> TargetChanged;
    public event Action<ITargetAction> TargetReached;

    public void DoStep(float movementStep)
    {

    }

    public void SetTarget(Vector3 targetPosition)
    {

    }

    private void FinishMovement()
    {
        if (_road != null)
        {
            if (_road.TryGetNextPoint(_currentPoint, out Vector3 nextPoint))
            {
                _currentPoint++;

                //TargetPosition = nextPoint;
                //SetTargetPosition(_targetPosition);
                //SetTargetRotation(TargetPosition);
            }
        }
        else
        {
            //base.FinishMovement();
        }
    }
}