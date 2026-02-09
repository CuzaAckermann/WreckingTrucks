using System;

public class TruckFactory : ModelFactory<Truck>
{
    private readonly GunFactory _gunFactory;
    private readonly TrunkCreator _trunkCreator;

    public TruckFactory(GunFactory gunFactory,
                        TrunkCreator trunkCreator,
                        FactorySettings factorySettings,
                        ModelSettings modelSettings)
                : base (factorySettings,
                        modelSettings)
    {
        _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        _trunkCreator = trunkCreator ?? throw new ArgumentNullException(nameof(trunkCreator));

        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    public override Truck Create()
    {
        Truck truck = base.Create();

        truck.SetGun(_gunFactory.Create());

        return truck;
    }

    protected override Truck CreateElement()
    {
        PositionManipulator positionManipulator = new PositionManipulator();

        return new Truck(positionManipulator,
                         MoverCreator.Create(positionManipulator),
                         RotatorCreator.Create(positionManipulator),
                         _trunkCreator.Create());
    }

    protected override IMoverCreator CreateMoverCreator()
    {
        return base.CreateMoverCreator();
    }

    protected override IRotatorCreator CreateRotatorCreator()
    {
        return base.CreateRotatorCreator();
    }
}