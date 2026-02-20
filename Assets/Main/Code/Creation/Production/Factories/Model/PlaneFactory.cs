using System;

public class PlaneFactory : ModelFactory<Plane>
{
    private readonly PlaneSettings _planeSettings;
    private readonly GunFactory _gunFactory;
    private readonly TrunkCreator _trunkCreator;

    public PlaneFactory(GunFactory gunFactory,
                        TrunkCreator trunkCreator,
                        PlaneFactorySettings planeFactorySettings,
                        ModelSettings modelSettings)
                : base (planeFactorySettings,
                        modelSettings)
    {
        Validator.ValidateNotNull(gunFactory, trunkCreator, planeFactorySettings.PlaneSettings);

        _gunFactory = gunFactory;
        _trunkCreator = trunkCreator;
        _planeSettings = planeFactorySettings.PlaneSettings;
    }

    public override IDestroyable Create()
    {
        if (Validator.IsRequiredType(base.Create(), out Plane plane))
        {
            throw new InvalidOperationException();
        }

        if (Validator.IsRequiredType(_gunFactory.Create(), out Gun gun) == false)
        {
            throw new InvalidOperationException();
        }

        plane.SetGun(gun);

        return plane;
    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        if (Validator.IsRequiredType(_trunkCreator.Create(), out Trunk trunk) == false)
        {
            throw new InvalidOperationException();
        }

        return new Plane(positionManipulator,
                         MoverCreator.Create(positionManipulator),
                         RotatorCreator.Create(positionManipulator),
                         trunk,
                         _planeSettings.AmountDestroyedRows);
    }
}