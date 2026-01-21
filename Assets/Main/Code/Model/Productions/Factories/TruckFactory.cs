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
        return new Truck(ModelSettings.Movespeed,
                         ModelSettings.Rotatespeed,
                         _trunkCreator.Create());
    }
}