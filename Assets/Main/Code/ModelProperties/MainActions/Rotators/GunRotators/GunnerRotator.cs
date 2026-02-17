public class GunnerRotator : Rotator
{
    private TurretRotator _turretRotator;

    public GunnerRotator(IRotatable rotatable, float rotationSpeed) : base(rotatable, rotationSpeed)
    {
    }

    public void SetTurretRotator(TurretRotator turretRotator)
    {
        Validator.ValidateNotNull(turretRotator);

        _turretRotator = turretRotator;
    }

    public void AimAtTarget(Placeable target)
    {
        _turretRotator.TargetReached += OnAimed;

        _turretRotator.SetTarget(target);
    }

    private void OnAimed(ITargetAction _)
    {
        _turretRotator.TargetReached -= OnAimed;

        OnTargetReached();
    }
}