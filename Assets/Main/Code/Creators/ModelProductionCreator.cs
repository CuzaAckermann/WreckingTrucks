using System;

public class ModelProductionCreator
{
    private readonly ModelFactoriesSettings _modelFactoriesSettings;

    private readonly GreenBlockFactory _greenBlockFactory;
    private readonly OrangeBlockFactory _orangeBlockFactory;
    private readonly PurpleBlockFactory _purpleBlockFactory;

    private readonly GreenTruckFactory _greenTruckFactory;
    private readonly OrangeTruckFactory _orangeTruckFactory;
    private readonly PurpleTruckFactory _purpleTruckFactory;

    private readonly CartrigeBoxFactory _cartrigeBoxFactory;

    private readonly GunFactory _gunFactory;

    private readonly BulletFactory _bulletFactory;

    private readonly PlaneFactory _planeFactory;

    public ModelProductionCreator(ModelFactoriesSettings modelFactoriesSettings,
                                  TrunkCreator trunkCreator,
                                  BlockTrackerCreator blockTrackerCreator,
                                  StopwatchCreator stopwatchCreator)
    {
        _modelFactoriesSettings = modelFactoriesSettings ? modelFactoriesSettings : throw new ArgumentNullException(nameof(modelFactoriesSettings));

        _gunFactory = new GunFactory(_modelFactoriesSettings.GunFactorySettings);
        _bulletFactory = new BulletFactory(_modelFactoriesSettings.BulletFactorySettings);

        _greenBlockFactory = new GreenBlockFactory(_modelFactoriesSettings.BlockFactorySettings);
        _orangeBlockFactory = new OrangeBlockFactory(_modelFactoriesSettings.BlockFactorySettings);
        _purpleBlockFactory = new PurpleBlockFactory(_modelFactoriesSettings.BlockFactorySettings);

        _greenTruckFactory = new GreenTruckFactory(_gunFactory,
                                                   trunkCreator,
                                                   blockTrackerCreator,
                                                   stopwatchCreator,
                                                   _modelFactoriesSettings.TruckFactorySettings);
        _orangeTruckFactory = new OrangeTruckFactory(_gunFactory,
                                                     trunkCreator,
                                                     blockTrackerCreator,
                                                     stopwatchCreator,
                                                     _modelFactoriesSettings.TruckFactorySettings);
        _purpleTruckFactory = new PurpleTruckFactory(_gunFactory,
                                                     trunkCreator,
                                                     blockTrackerCreator,
                                                     stopwatchCreator,
                                                     _modelFactoriesSettings.TruckFactorySettings);

        _cartrigeBoxFactory = new CartrigeBoxFactory(_modelFactoriesSettings.CartrigeBoxSettings);

        _planeFactory = new PlaneFactory(_gunFactory,
                                         trunkCreator,
                                         stopwatchCreator,
                                         _modelFactoriesSettings.PlaneFactorySettings);
    }

    public ModelProduction<Block> CreateBlockProduction()
    {
        ModelProduction<Block> blockProduction = new ModelProduction<Block>();

        blockProduction.AddFactory<GreenBlock>(_greenBlockFactory);
        blockProduction.AddFactory<OrangeBlock>(_orangeBlockFactory);
        blockProduction.AddFactory<PurpleBlock>(_purpleBlockFactory);

        return blockProduction;
    }

    public ModelProduction<Truck> CreateTruckProduction()
    {
        ModelProduction<Truck> truckProduction = new ModelProduction<Truck>();

        truckProduction.AddFactory<GreenTruck>(_greenTruckFactory);
        truckProduction.AddFactory<OrangeTruck>(_orangeTruckFactory);
        truckProduction.AddFactory<PurpleTruck>(_purpleTruckFactory);

        return truckProduction;
    }

    public ModelProduction<CartrigeBox> CreateCartrigeBoxProduction()
    {
        ModelProduction<CartrigeBox> cartrigeBoxProduction = new ModelProduction<CartrigeBox>();

        cartrigeBoxProduction.AddFactory<CartrigeBox>(_cartrigeBoxFactory);

        return cartrigeBoxProduction;
    }

    public BulletFactory CreateBulletFactory()
    {
        return _bulletFactory;
    }

    public PlaneFactory CreatePlaneFactory()
    {
        return _planeFactory;
    }
}