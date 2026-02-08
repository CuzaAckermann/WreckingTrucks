using System;

public class Gunner : Model
{
    public Gunner(PositionManipulator positionManipulator, IMover mover, IRotator rotator)
           : base(positionManipulator, mover, rotator)
    {
        
    }

    public event Action Aimed;

    public Turret Turret { get; private set; }

    public void Prepare(Turret turret)
    {
        Turret = turret ?? throw new ArgumentNullException(nameof(turret));
    }

    public override void Destroy()
    {
        Turret.Destroy();

        base.Destroy();
    }

    public void AimAtTarget(Model target)
    {
        Turret.Aimed += OnAimed;

        Turret.SetTarget(target);
    }

    private void OnAimed()
    {
        Turret.Aimed -= OnAimed;

        Aimed?.Invoke();
    }
}