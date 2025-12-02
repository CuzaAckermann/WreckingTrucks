using System;

public class TruckFactory : ModelFactory<Truck>
{
    private readonly GunFactory _gunFactory;
    private readonly TrunkCreator _trunkCreator;

    private readonly BlockTrackerCreator _blockTrackerCreator;

    public TruckFactory(GunFactory gunFactory,
                        TrunkCreator trunkCreator,
                        FactorySettings factorySettings,
                        ModelSettings modelSettings,
                        BlockTrackerCreator blockTrackerCreator)
                : base (factorySettings,
                        modelSettings)
    {
        _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        _trunkCreator = trunkCreator ?? throw new ArgumentNullException(nameof(trunkCreator));
        _blockTrackerCreator = blockTrackerCreator ?? throw new ArgumentNullException(nameof(blockTrackerCreator));

        InitializePool(factorySettings.InitialPoolSize,
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
        return new Truck(ModelSettings.Movespeed,
                         ModelSettings.Rotatespeed,
                         _trunkCreator.Create(),
                         _blockTrackerCreator.Create());
    }
}