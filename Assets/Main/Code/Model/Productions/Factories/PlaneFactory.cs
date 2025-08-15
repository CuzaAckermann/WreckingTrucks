using System;

public class PlaneFactory : ModelFactory<Plane>
{
    private readonly PlaneSettings _planeSettings;
    private readonly GunFactory _gunFactory;
    private readonly TrunkCreator _trunkCreator;
    private readonly StopwatchCreator _stopwatchCreator;

    public PlaneFactory(GunFactory gunFactory,
                        TrunkCreator trunkCreator,
                        StopwatchCreator stopwatchCreator,
                        PlaneFactorySettings planeFactorySettings)
    {
        _gunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        _trunkCreator = trunkCreator ?? throw new ArgumentNullException(nameof(trunkCreator));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _planeSettings = planeFactorySettings.PlaneSettings;

        InitializePool(planeFactorySettings.InitialPoolSize,
                       planeFactorySettings.MaxPoolCapacity);
    }

    protected override Plane CreateElement()
    {
        return new Plane(_gunFactory.Create(),
                         _trunkCreator.Create(),
                         _stopwatchCreator.Create(),
                         _planeSettings.ShotCooldown,
                         _planeSettings.AmountDestroyedRows,
                         _planeSettings.GunPosition,
                         _planeSettings.TrunkPosition);
    }
}