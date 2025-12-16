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
        _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        _trunkCreator = trunkCreator ?? throw new ArgumentNullException(nameof(trunkCreator));
        _planeSettings = planeFactorySettings.PlaneSettings;

        InitPool(planeFactorySettings.InitialPoolSize,
                 planeFactorySettings.MaxPoolCapacity);
    }

    public override Plane Create()
    {
        Plane plane = base.Create();

        plane.SetGun(_gunFactory.Create());

        return plane;
    }

    protected override Plane CreateElement()
    {
        return new Plane(ModelSettings.Movespeed,
                         ModelSettings.Rotatespeed,
                         _trunkCreator.Create(),
                         _planeSettings.AmountDestroyedRows);
    }
}