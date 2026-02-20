using System;

public class GunnerFactory : ModelFactory<Gunner>
{
    private readonly TurretFactory _turretFactory;

    public GunnerFactory(FactorySettings factorySettings,
                         ModelSettings modelSettings,
                         TurretFactory turretFactory)
                  : base(factorySettings,
                         modelSettings)
    {
        Validator.ValidateNotNull(turretFactory);

        _turretFactory = turretFactory;
    }

    public override IDestroyable Create()
    {
        if (Validator.IsRequiredType(base.Create(), out Gunner gunner) == false)
        {
            throw new InvalidOperationException();
        }

        if (Validator.IsRequiredType(_turretFactory.Create(), out Turret turret) == false)
        {
            throw new InvalidOperationException();
        }

        gunner.Prepare(turret);

        return gunner;
    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new Gunner(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          new GunnerRotator(positionManipulator, 10));
    }
}