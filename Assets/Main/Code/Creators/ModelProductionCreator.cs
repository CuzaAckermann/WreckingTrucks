using System;

public class ModelProductionCreator
{
    private readonly ModelFactoriesSettings _modelFactoriesSettings;
    private readonly ModelsSettings _modelsSettings;

    private readonly BlockFactory _blockFactory;
    private readonly TruckFactory _truckFactory;
    private readonly CartrigeBoxFactory _cartrigeBoxFactory;
    private readonly GunFactory _gunFactory;
    private readonly BulletFactory _bulletFactory;
    private readonly PlaneFactory _planeFactory;

    private readonly ModelProduction _modelProduction;

    public ModelProductionCreator(ModelFactoriesSettings modelFactoriesSettings,
                                  ModelsSettings modelsSettings,
                                  TrunkCreator trunkCreator,
                                  StopwatchCreator stopwatchCreator,
                                  BlockTrackerCreator blockTrackerCreator)
    {
        _modelFactoriesSettings = modelFactoriesSettings ? modelFactoriesSettings : throw new ArgumentNullException(nameof(modelFactoriesSettings));
        _modelsSettings = modelsSettings ? modelsSettings : throw new ArgumentNullException(nameof(modelsSettings));

        _bulletFactory = new BulletFactory(_modelFactoriesSettings.BulletFactorySettings,
                                           _modelsSettings.BulletSettings);

        _gunFactory = new GunFactory(_bulletFactory,
                                     _modelFactoriesSettings.GunFactorySettings,
                                     stopwatchCreator);

        _blockFactory = new BlockFactory(_modelFactoriesSettings.BlockFactorySettings,
                                         _modelsSettings.BlockSettings);

        _truckFactory = new TruckFactory(_gunFactory,
                                         trunkCreator,
                                         _modelFactoriesSettings.TruckFactorySettings,
                                         _modelsSettings.TruckSettings,
                                         blockTrackerCreator);

        _cartrigeBoxFactory = new CartrigeBoxFactory(_modelFactoriesSettings.CartrigeBoxSettings,
                                                     _modelsSettings.CartrigeBoxSettings);

        _planeFactory = new PlaneFactory(_gunFactory,
                                         trunkCreator,
                                         stopwatchCreator,
                                         _modelFactoriesSettings.PlaneFactorySettings,
                                         _modelsSettings.PlaneSettings);

        _modelProduction = new ModelProduction();

        FillModelProduction();
    }

    public ModelProduction CreateModelProduction()
    {
        return _modelProduction;
    }

    public BlockFactory CreateBlockFactory()
    {
        return _blockFactory;
    }

    public TruckFactory CreateTruckFactory()
    {
        return _truckFactory;
    }

    public CartrigeBoxFactory CreateCartrigeBoxFactory()
    {
        return _cartrigeBoxFactory;
    }

    public BulletFactory CreateBulletFactory()
    {
        return _bulletFactory;
    }

    public PlaneFactory CreatePlaneFactory()
    {
        return _planeFactory;
    }

    private void FillModelProduction()
    {
        _modelProduction.AddFactory(_blockFactory);
        _modelProduction.AddFactory(_truckFactory);
        _modelProduction.AddFactory(_gunFactory);
        _modelProduction.AddFactory(_cartrigeBoxFactory);
        _modelProduction.AddFactory(_bulletFactory);
        _modelProduction.AddFactory(_planeFactory);

        _modelProduction.SubscribeToFactories();
    }
}