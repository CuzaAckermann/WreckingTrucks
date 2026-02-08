using System;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Model
{
    private readonly float _rotateSpeed;

    private bool _isCompleted;
    private bool _isBarrelCompleted;

    private Model _currentTarget;

    public Turret(PositionManipulator positionManipulator,
                  IMover mover,
                  IRotator rotator,
                  float rotationSpeed)
           : base(positionManipulator, mover, rotator)
    {
        if (rotationSpeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rotationSpeed));
        }

        _rotateSpeed = rotationSpeed;
    }

    public event Action Aimed;

    public Barrel Barrel { get; private set; }

    public void SetBarrel(Barrel barrel)
    {
        Barrel = barrel ?? throw new ArgumentNullException(nameof(barrel));
    }

    public void SetTarget(Model target)
    {
        _currentTarget = target;

        Barrel.Aimed += OnAimed;
        Barrel.SetTarget(target);

        SetTargetRotation(_currentTarget.Position);
    }

    public override void Destroy()
    {
        Barrel.Destroy();

        base.Destroy();
    }

    public override void Rotate(float frameRotation)
    {
        SetTargetRotation(_currentTarget.Position);

        float rotationAmount = frameRotation * _rotateSpeed;

        Vector3 projectOnPlaneYCurrentTarget = new Vector3(_currentTarget.Position.x, 0, _currentTarget.Position.z);
        Vector3 projectOnPlaneYPosition = new Vector3(Position.x, 0, Position.z);

        Vector3 direction = (projectOnPlaneYCurrentTarget - projectOnPlaneYPosition).normalized;

        float angle = Vector3.Angle(Forward, direction);

        if (angle <= rotationAmount)
        {
            rotationAmount = angle;

            _isCompleted = true;
        }

        Vector3 cross = Vector3.Cross(Forward, direction);

        rotationAmount = cross.y > 0 ? rotationAmount : -rotationAmount;

        Quaternion rotation = Quaternion.AngleAxis(rotationAmount, Vector3.up);

        Barrel.RotateQuaternion(rotation);

        SetDirectionForward(rotation * Forward);

        if (IsAimed())
        {
            Aimed?.Invoke();
        }
    }

    private bool IsAimed()
    {
        return _isCompleted && _isBarrelCompleted;
    }

    private void OnAimed()
    {
        Barrel.Aimed -= OnAimed;

        _isBarrelCompleted = true;

        if (IsAimed())
        {
            Aimed?.Invoke();
        }
    }
}