using System;

public abstract class TruckFactory<T> : ModelFactory<T> where T : Truck
{
    protected readonly TruckSettings TruckSettings;
    protected readonly GunFactory GunFactory;
    protected readonly TrunkCreator TrunkCreator;
    protected readonly BlockTrackerCreator BlockTrackerCreator;
    protected readonly StopwatchCreator StopwatchCreator;

    public TruckFactory(GunFactory gunFactory,
                        TrunkCreator trunkCreator,
                        BlockTrackerCreator blockTrackerCreator,
                        StopwatchCreator stopwatchCreator,
                        TruckFactorySettings factorySettings)
    {
        GunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        TrunkCreator = trunkCreator ?? throw new ArgumentNullException(nameof(trunkCreator));
        BlockTrackerCreator = blockTrackerCreator ?? throw new ArgumentNullException(nameof(blockTrackerCreator));
        StopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        TruckSettings = factorySettings.TruckSettings;

        InitializePool(factorySettings.InitialPoolSize,
                       factorySettings.MaxPoolCapacity);
    }
}