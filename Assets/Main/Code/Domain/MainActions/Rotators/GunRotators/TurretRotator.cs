using UnityEngine;

public class TurretRotator : Rotator
{
    private BarrelRotator _barrelRotator;

    private bool _isCompleted;
    private bool _isBarrelCompleted;

    private PositionManipulator _target;

    public TurretRotator(IRotatable rotatable, float rotationSpeed) : base(rotatable, rotationSpeed)
    {
        
    }

    public void SetBarrelRotator(BarrelRotator barrelRotator)
    {
        Validator.ValidateNotNull(barrelRotator);

        _barrelRotator = barrelRotator;
    }

    public void SetTarget(PositionManipulator target)
    {
        Validator.ValidateNotNull(target);

        _target = target;

        _barrelRotator.TargetReached += OnTargetReached;
        _barrelRotator.SetTarget(_target);

        SetTarget(_target.Position);

        //OnTargetChanged();
    }

    public override void DoStep(float rotationStep)
    {
        SetTarget(_target.Position);

        float rotationAmount = rotationStep * RotationSpeed;

        Vector3 projectOnPlaneYCurrentTarget = new Vector3(_target.Position.x, 0, _target.Position.z);
        Vector3 projectOnPlaneYPosition = new Vector3(Rotatable.Position.x, 0, Rotatable.Position.z);

        Vector3 direction = (projectOnPlaneYCurrentTarget - projectOnPlaneYPosition).normalized;

        float angle = Vector3.Angle(Rotatable.Forward, direction);

        if (angle <= rotationAmount)
        {
            rotationAmount = angle;

            _isCompleted = true;
        }

        Vector3 cross = Vector3.Cross(Rotatable.Forward, direction);

        rotationAmount = cross.y > 0 ? rotationAmount : -rotationAmount;

        Quaternion rotation = Quaternion.AngleAxis(rotationAmount, Vector3.up);

        _barrelRotator.RotateQuaternion(rotation);

        Rotatable.SetForward(rotation * Rotatable.Forward);

        if (IsAimed())
        {
            _isBarrelCompleted = false;

            OnTargetReached();
        }
    }

    private bool IsAimed()
    {
        return _isCompleted && _isBarrelCompleted;
    }

    private void OnTargetReached(ITargetAction _)
    {
        _barrelRotator.TargetReached -= OnTargetReached;

        _isBarrelCompleted = true;

        if (IsAimed())
        {
            OnTargetReached();
        }
    }
}