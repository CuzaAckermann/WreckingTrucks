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
        Validator.ValidateNotNull(gunFactory, trunkCreator);

        _gunFactory = gunFactory;
        _trunkCreator = trunkCreator;
    }

    public override IDestroyable Create()
    {
        if (Validator.IsRequiredType(base.Create(), out Truck truck) == false)
        {
            throw new InvalidOperationException();
        }

        if (Validator.IsRequiredType(_gunFactory.Create(), out Gun gun) == false)
        {
            throw new InvalidOperationException();
        }

        truck.SetGun(gun);

        return truck;
    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        if (Validator.IsRequiredType(_trunkCreator.Create(), out Trunk trunk) == false)
        {
            throw new InvalidOperationException();
        }

        return new Truck(positionManipulator,
                         MoverCreator.Create(positionManipulator),
                         RotatorCreator.Create(positionManipulator),
                         trunk);
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