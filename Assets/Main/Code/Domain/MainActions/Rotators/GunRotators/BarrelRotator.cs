using UnityEngine;

public class BarrelRotator : Rotator
{
    private Vector3 _right;

    private PositionManipulator _target;

    public BarrelRotator(IRotatable rotatable, float rotatespeed) : base(rotatable, rotatespeed)
    {

    }

    public void SetRight(Vector3 right)
    {
        _right = right;
    }

    public void SetTarget(PositionManipulator target)
    {
        Validator.ValidateNotNull(target);

        _target = target;

        OnTargetChanged();
    }

    public void RotateQuaternion(Quaternion rotation)
    {
        Rotatable.RotateForward(rotation);
    }

    public override void DoStep(float rotationStep)
    {
        SetTarget(_target.Position);

        float rotationAmount = rotationStep * RotationSpeed;

        Vector3 direction = (_target.Position - Rotatable.Position).normalized;

        Vector3 projectDirectionOnYPlane = Vector3.ProjectOnPlane(direction, Vector3.up);

        Vector3 futureRight = Vector3.Cross(projectDirectionOnYPlane, Vector3.up).normalized;

        Vector3 projectForwardOnPlaneY = Vector3.ProjectOnPlane(Rotatable.Forward, Vector3.up);

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

        Vector3 virtualForward = rotationVirtualForward * Rotatable.Forward;

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

        Rotatable.RotateForward(rotation);

        if (isFinished)
        {
            OnTargetReached();
        }
    }
}