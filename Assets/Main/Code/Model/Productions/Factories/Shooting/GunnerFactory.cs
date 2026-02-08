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
        _turretFactory = turretFactory ?? throw new ArgumentNullException(nameof(turretFactory));

        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    public override Gunner Create()
    {
        Gunner gunner = base.Create();

        gunner.Prepare(_turretFactory.Create());

        return gunner;
    }

    protected override Gunner CreateElement()
    {
        PositionManipulator positionManipulator = new PositionManipulator();

        return new Gunner(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          RotatorCreator.Create(positionManipulator));
    }
}