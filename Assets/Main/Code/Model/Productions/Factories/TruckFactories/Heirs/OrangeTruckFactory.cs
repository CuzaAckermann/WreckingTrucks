public class OrangeTruckFactory : TruckFactory<OrangeTruck>
{
    public OrangeTruckFactory(GunFactory gunFactory,
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

    protected override OrangeTruck CreateElement()
    {
        OrangeTruck orangeTruck = new OrangeTruck(GunFactory.Create(),
                                                  TrunkCreator.Create(),
                                                  BlockTrackerCreator.Create(),
                                                  StopwatchCreator.Create(),
                                                  TruckSettings.ShotCooldown,
                                                  TruckSettings.GunPosition,
                                                  TruckSettings.TrunkPosition);
        orangeTruck.SetDestroyableType<OrangeBlock>();

        return orangeTruck;
    }
}