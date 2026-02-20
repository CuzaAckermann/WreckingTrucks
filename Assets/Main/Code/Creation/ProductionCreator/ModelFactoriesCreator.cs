using System.Collections.Generic;

public class ModelFactoriesCreator
{
    private readonly ModelFactoriesSettings _modelFactoriesSettings;
    private readonly ModelsSettings _modelsSettings;
    private readonly EventBus _eventBus;

    public ModelFactoriesCreator(ModelFactoriesSettings modelFactoriesSettings,
                                 ModelsSettings modelsSettings,
                                 EventBus eventBus)
    {
        Validator.ValidateNotNull(modelFactoriesSettings, modelsSettings, eventBus);

        _modelFactoriesSettings = modelFactoriesSettings;
        _modelsSettings = modelsSettings;
        _eventBus = eventBus;
    }

    public List<Factory> Create()
    {
        TrunkCreator trunkCreator = new TrunkCreator(_modelFactoriesSettings.TrunkFactorySettings,
                                                                              _modelsSettings.TrunkSettings);

        BulletFactory bulletFactory = new BulletFactory(_modelFactoriesSettings.BulletFactorySettings,
                                                        _modelsSettings.BulletSettings);

        BarrelFactory barrelFactory = new BarrelFactory(_modelFactoriesSettings.BarrelFactorySettings,
                                                        _modelsSettings.BarrelSettings);

        TurretFactory turretFactory = new TurretFactory(_modelFactoriesSettings.TurretFactorySetting,
                                                        _modelsSettings.TurretSettings,
                                                        barrelFactory);

        GunnerFactory gunnerFactory = new GunnerFactory(_modelFactoriesSettings.GunnerFactorySettings,
                                                        _modelsSettings.GunnerSettings,
                                                        turretFactory);

        GunFactory gunFactory = new GunFactory(_eventBus,
                                               bulletFactory,
                                               _modelFactoriesSettings.GunFactorySettings,
                                               gunnerFactory);

        BlockFactory blockFactory = new BlockFactory(_modelFactoriesSettings.BlockFactorySettings,
                                                     _modelsSettings.BlockSettings);

        TruckFactory truckFactory = new TruckFactory(gunFactory,
                                                     trunkCreator,
                                                     _modelFactoriesSettings.TruckFactorySettings,
                                                     _modelsSettings.TruckSettings);

        CartrigeBoxFactory cartrigeBoxFactory = new CartrigeBoxFactory(_modelFactoriesSettings.CartrigeBoxSettings,
                                                                       _modelsSettings.CartrigeBoxSettings);

        PlaneFactory planeFactory = new PlaneFactory(gunFactory,
                                                     trunkCreator,
                                                     _modelFactoriesSettings.PlaneFactorySettings,
                                                     _modelsSettings.PlaneSettings);

        return new List<Factory>
        {
            blockFactory,
            truckFactory,
            cartrigeBoxFactory,
            gunFactory,
            bulletFactory,
            planeFactory,
            gunnerFactory,
            turretFactory,
            barrelFactory
        };
    }
}