using System;

public class Turret : Model
{
    public Turret(Placeable positionManipulator,
                  IMover mover,
                  TurretRotator rotator)
           : base(positionManipulator, mover, rotator)
    {
        
    }

    public Barrel Barrel { get; private set; }

    public void SetBarrel(Barrel barrel)
    {
        Barrel = barrel ?? throw new ArgumentNullException(nameof(barrel));
    }

    public override void Destroy()
    {
        Barrel.Destroy();

        base.Destroy();
    }

    //public override void Rotate(float frameRotation)
    //{
    //    Rotator.SetTarget(_currentTarget.PositionManipulator.Position);

    //    float rotationAmount = frameRotation * _rotateSpeed;

    //    Vector3 projectOnPlaneYCurrentTarget = new Vector3(_currentTarget.PositionManipulator.Position.x, 0, _currentTarget.PositionManipulator.Position.z);
    //    Vector3 projectOnPlaneYPosition = new Vector3(PositionManipulator.Position.x, 0, PositionManipulator.Position.z);

    //    Vector3 direction = (projectOnPlaneYCurrentTarget - projectOnPlaneYPosition).normalized;

    //    float angle = Vector3.Angle(PositionManipulator.Forward, direction);

    //    if (angle <= rotationAmount)
    //    {
    //        rotationAmount = angle;

    //        _isCompleted = true;
    //    }

    //    Vector3 cross = Vector3.Cross(PositionManipulator.Forward, direction);

    //    rotationAmount = cross.y > 0 ? rotationAmount : -rotationAmount;

    //    Quaternion rotation = Quaternion.AngleAxis(rotationAmount, Vector3.up);

    //    Barrel.RotateQuaternion(rotation);

    //    SetDirectionForward(rotation * PositionManipulator.Forward);

    //    if (IsAimed())
    //    {
    //        Aimed?.Invoke();
    //    }
    //}

    //private bool IsAimed()
    //{
    //    return _isCompleted && _isBarrelCompleted;
    //}

    //private void OnAimed()
    //{
    //    Barrel.Aimed -= OnAimed;

    //    _isBarrelCompleted = true;

    //    if (IsAimed())
    //    {
    //        Aimed?.Invoke();
    //    }
    //}
}