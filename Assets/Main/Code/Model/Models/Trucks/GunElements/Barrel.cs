using System;
using UnityEngine;

public class Barrel : Model
{
    private readonly float _rotationSpeed;

    private Vector3 _right;

    private Model _currentTarget;

    public Barrel(PositionManipulator positionManipulator,
                  IMover mover,
                  IRotator rotator,
                  float rotationSpeed)
           : base(positionManipulator, mover, rotator)
    {
        if (rotationSpeed <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rotationSpeed));
        }

        _rotationSpeed = rotationSpeed;
    }

    public event Action Aimed;

    public void SetRight(Vector3 right)
    {
        _right = right;
    }

    public void SetTarget(Model target)
    {
        _currentTarget = target;

        SetTargetRotation(_currentTarget.Position);
    }

    public void RotateQuaternion(Quaternion quaternion)
    {
        SetDirectionForward(quaternion * Forward);
    }

    public override void Rotate(float frameRotation)
    {
        SetTargetRotation(_currentTarget.Position);

        float rotationAmount = frameRotation * _rotationSpeed;

        Vector3 direction = (_currentTarget.Position - Position).normalized;

        Vector3 projectDirectionOnYPlane = Vector3.ProjectOnPlane(direction, Vector3.up);

        Vector3 futureRight = Vector3.Cross(projectDirectionOnYPlane, Vector3.up).normalized;

        Vector3 projectForwardOnPlaneY = Vector3.ProjectOnPlane(Forward, Vector3.up);

        float angleBetweenForwardAndDirection = Vector3.Angle(projectForwardOnPlaneY,
                                                              projectDirectionOnYPlane);

        Vector3 crossProjects = Vector3.Cross(projectForwardOnPlaneY,
                                              projectDirectionOnYPlane);

        if (crossProjects.y < 0)
        {
            angleBetweenForwardAndDirection *= -1;
        }

        Quaternion rotationVirtualForward = Quaternion.AngleAxis(angleBetweenForwardAndDirection,
                                                                 Vector3.up);

        Vector3 virtualForward = rotationVirtualForward * Forward;

        float angle = Vector3.Angle(virtualForward, direction);

        bool isFinished = angle <= rotationAmount;

        if (isFinished)
        {
            rotationAmount = angle;
        }

        Vector3 cross = Vector3.Cross(virtualForward, direction);

        float dot = Vector3.Dot(cross, futureRight);

        if (dot > 0)
        {
            rotationAmount *= -1;
        }

        Quaternion rotation = Quaternion.AngleAxis(rotationAmount, _right);

        SetDirectionForward(rotation * Forward);

        if (isFinished)
        {
            Aimed?.Invoke();
        }
    }
}