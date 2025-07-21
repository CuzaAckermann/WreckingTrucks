public class PurpleTruckFactory : TruckFactory<PurpleTruck>
{
    public PurpleTruckFactory(GunFactory gunFactory,
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

    protected override PurpleTruck CreateElement()
    {
        PurpleTruck purpleTruck = new PurpleTruck(GunFactory.Create(),
                                                  TrunkCreator.Create(),
                                                  BlockTrackerCreator.Create(),
                                                  StopwatchCreator.Create(),
                                                  TruckSettings.ShotCooldown,
                                                  TruckSettings.GunPosition,
                                                  TruckSettings.TrunkPosition);
        purpleTruck.SetDestroyableType<PurpleBlock>();

        return purpleTruck;
    }
}