using System;

public class Gunner : Model
{
    public Gunner(PositionManipulator positionManipulator, IMover mover, GunnerRotator rotator)
           : base(positionManipulator, mover, rotator)
    {
        
    }

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

    public void AimAtTarget(PositionManipulator target)
    {
        if (Rotator is GunnerRotator gunnerRotator)
        {
            gunnerRotator.AimAtTarget(target);
        }
    }
}