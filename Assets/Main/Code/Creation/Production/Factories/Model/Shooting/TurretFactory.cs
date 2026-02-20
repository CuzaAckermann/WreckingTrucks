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
        Validator.ValidateNotNull(barrelFactory);

        _barrelFactory = barrelFactory;
    }

    public override IDestroyable Create()
    {
        if (Validator.IsRequiredType(base.Create(), out Turret turret) == false)
        {
            throw new InvalidOperationException();
        }

        if (Validator.IsRequiredType(_barrelFactory.Create(), out Barrel barrel) == false)
        {
            throw new InvalidOperationException();
        }

        turret.SetBarrel(barrel);

        return turret;
    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new Turret(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          new TurretRotator(positionManipulator, ModelSettings.RotationSpeed));
    }
}