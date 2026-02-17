using UnityEngine;

public class Barrel : Model
{
    public Barrel(Placeable positionManipulator,
                  IMover mover,
                  BarrelRotator rotator)
           : base(positionManipulator, mover, rotator)
    {

    }

    public void SetRight(Vector3 right)
    {
        if (Rotator is BarrelRotator barrelRotator)
        {
            barrelRotator.SetRight(right);
        }
    }

    //public override void Rotate(float frameRotation)
    //{
    //    Rotator.SetTarget(_currentTarget.PositionManipulator.Position);

    //    float rotationAmount = frameRotation * _rotationSpeed;

    //    Vector3 direction = (_currentTarget.PositionManipulator.Position - PositionManipulator.Position).normalized;

    //    Vector3 projectDirectionOnYPlane = Vector3.ProjectOnPlane(direction, Vector3.up);

    //    Vector3 futureRight = Vector3.Cross(projectDirectionOnYPlane, Vector3.up).normalized;

    //    Vector3 projectForwardOnPlaneY = Vector3.ProjectOnPlane(PositionManipulator.Forward, Vector3.up);

    //    float angleBetweenForwardAndDirection = Vector3.Angle(projectForwardOnPlaneY,
    //                                                          projectDirectionOnYPlane);

    //    Vector3 crossProjects = Vector3.Cross(projectForwardOnPlaneY,
    //                                          projectDirectionOnYPlane);

    //    if (crossProjects.y < 0)
    //    {
    //        angleBetweenForwardAndDirection *= -1;
    //    }

    //    Quaternion rotationVirtualForward = Quaternion.AngleAxis(angleBetweenForwardAndDirection,
    //                                                             Vector3.up);

    //    Vector3 virtualForward = rotationVirtualForward * PositionManipulator.Forward;

    //    float angle = Vector3.Angle(virtualForward, direction);

    //    bool isFinished = angle <= rotationAmount;

    //    if (isFinished)
    //    {
    //        rotationAmount = angle;
    //    }

    //    Vector3 cross = Vector3.Cross(virtualForward, direction);

    //    float dot = Vector3.Dot(cross, futureRight);

    //    if (dot > 0)
    //    {
    //        rotationAmount *= -1;
    //    }

    //    Quaternion rotation = Quaternion.AngleAxis(rotationAmount, _right);

    //    SetDirectionForward(rotation * PositionManipulator.Forward);

    //    if (isFinished)
    //    {
    //        Aimed?.Invoke();
    //    }
    //}
}