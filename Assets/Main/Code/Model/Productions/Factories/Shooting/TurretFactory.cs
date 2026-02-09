using System;

public class TurretFactory : ModelFactory<Turret>
{
    private readonly BarrelFactory _barrelFactory;

    public TurretFactory(FactorySettings factorySettings,
                         ModelSettings modelSettings,
                         BarrelFactory barrelFactory)
                  : base(factorySettings,
                         modelSettings)
    {
        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);

        _barrelFactory = barrelFactory ?? throw new ArgumentNullException(nameof(barrelFactory));
    }

    public override Turret Create()
    {
        Turret turret = base.Create();

        turret.SetBarrel(_barrelFactory.Create());

        return turret;
    }

    protected override Turret CreateElement()
    {
        PositionManipulator positionManipulator = new PositionManipulator();

        return new Turret(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          new TurretRotator(positionManipulator, ModelSettings.RotationSpeed));
    }
}