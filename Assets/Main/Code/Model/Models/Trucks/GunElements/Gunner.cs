using System;

public class Gunner : Model
{
    public Gunner(Placeable positionManipulator, IMover mover, GunnerRotator rotator)
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

    public void AimAtTarget(Placeable target)
    {
        if (Rotator is GunnerRotator gunnerRotator)
        {
            gunnerRotator.AimAtTarget(target);
        }
    }
}