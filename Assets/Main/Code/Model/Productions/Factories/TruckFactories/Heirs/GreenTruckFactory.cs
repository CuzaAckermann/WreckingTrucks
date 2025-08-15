public class GreenTruckFactory : TruckFactory<GreenTruck>
{
    public GreenTruckFactory(GunFactory gunFactory,
                             TrunkCreator trunkCreator,
                             BlockTrackerCreator blockTrackerCreator,
                             StopwatchCreator stopwatchCreator,
                             TruckFactorySettings factorySettings)
                      : base(gunFactory,
                             trunkCreator,
                             blockTrackerCreator,
                             stopwatchCreator,
                             factorySettings)
    {

    }

    protected override GreenTruck CreateElement()
    {
        GreenTruck greenTruck = new GreenTruck(GunFactory.Create(),
                                               TrunkCreator.Create(),
                                               BlockTrackerCreator.Create(),
                                               StopwatchCreator.Create(),
                                               TruckSettings.ShotCooldown);
        greenTruck.SetDestroyableType<GreenBlock>();

        return greenTruck;
    }
}