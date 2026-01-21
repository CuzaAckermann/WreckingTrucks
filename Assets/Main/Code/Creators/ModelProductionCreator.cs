using System;
using System.Collections.Generic;

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

    private readonly GunnerFactory _gunnerFactory;
    private readonly TurretFactory _turretFactory;
    private readonly BarrelFactory _barrelFactory;

    private readonly ModelProduction _modelProduction;

    private List<ICreator<Model>> _factories;

    public ModelProductionCreator(ModelFactoriesSettings modelFactoriesSettings,
                                  ModelsSettings modelsSettings,
                                  TrunkCreator trunkCreator,
                                  StopwatchCreator stopwatchCreator,
                                  EventBus eventBus)
    {
        _modelFactoriesSettings = modelFactoriesSettings ? modelFactoriesSettings : throw new ArgumentNullException(nameof(modelFactoriesSettings));
        _modelsSettings = modelsSettings ? modelsSettings : throw new ArgumentNullException(nameof(modelsSettings));

        _bulletFactory = new BulletFactory(_modelFactoriesSettings.BulletFactorySettings,
                                           _modelsSettings.BulletSettings);

        _barrelFactory = new BarrelFactory(_modelFactoriesSettings.BarrelFactorySettings,
                                           _modelsSettings.BarrelSettings);

        _turretFactory = new TurretFactory(_modelFactoriesSettings.TurretFactorySetting,
                                           _modelsSettings.TurretSettings,
                                           _barrelFactory);
        
        _gunnerFactory = new GunnerFactory(_modelFactoriesSettings.GunnerFactorySettings,
                                           _modelsSettings.GunnerSettings,
                                           _turretFactory);

        _gunFactory = new GunFactory(_bulletFactory,
                                     _modelFactoriesSettings.GunFactorySettings,
                                     stopwatchCreator,
                                     _gunnerFactory);

        _blockFactory = new BlockFactory(_modelFactoriesSettings.BlockFactorySettings,
                                         _modelsSettings.BlockSettings);

        _truckFactory = new TruckFactory(_gunFactory,
                                         trunkCreator,
                                         _modelFactoriesSettings.TruckFactorySettings,
                                         _modelsSettings.TruckSettings);

        _cartrigeBoxFactory = new CartrigeBoxFactory(_modelFactoriesSettings.CartrigeBoxSettings,
                                                     _modelsSettings.CartrigeBoxSettings);

        _planeFactory = new PlaneFactory(_gunFactory,
                                         trunkCreator,
                                         _modelFactoriesSettings.PlaneFactorySettings,
                                         _modelsSettings.PlaneSettings);

        _modelProduction = new ModelProduction(eventBus);

        FillModelProduction();

        FillCreators();
    }

    public ModelFactory<M> CreateFactory<M>() where M : Model
    {
        foreach (var modelFactory in _factories)
        {
            if (modelFactory.CreatableType == typeof(M))
            {
                if (modelFactory is ModelFactory<M> contextFactory)
                {
                    return contextFactory;
                }
            }
        }

        return null;
    }

    private void FillModelProduction()
    {
        _modelProduction.AddFactory(_blockFactory);
        _modelProduction.AddFactory(_truckFactory);
        _modelProduction.AddFactory(_cartrigeBoxFactory);
        _modelProduction.AddFactory(_gunFactory);
        _modelProduction.AddFactory(_bulletFactory);
        _modelProduction.AddFactory(_planeFactory);

        _modelProduction.AddFactory(_gunnerFactory);
        _modelProduction.AddFactory(_turretFactory);
        _modelProduction.AddFactory(_barrelFactory);

        _modelProduction.SubscribeToFactories();
    }

    private void FillCreators()
    {
        _factories = new List<ICreator<Model>>
        {
            _blockFactory,
            _truckFactory,
            _cartrigeBoxFactory,
            _gunFactory,
            _bulletFactory,
            _planeFactory,
            _gunnerFactory,
            _turretFactory,
            _barrelFactory
        };
    }
}