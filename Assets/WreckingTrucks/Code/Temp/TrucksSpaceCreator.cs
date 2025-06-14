using UnityEngine;

public class TrucksSpaceCreator : SpaceCreator<Truck, TruckFactory>
{
    [Header("Presenter Factories")]
    [SerializeField] private GreenTruckPresenterFactory _greenTruckPresenterFactory;
    [SerializeField] private OrangeTruckPresenterFactory _orangeTruckPresenterFactory;
    [SerializeField] private PurpleTruckPresenterFactory _purpleTruckPresenterFactory;

    //[Header("Gun Factory Settings")]
    //[SerializeField] private int _initialPoolSizeForGunFactory;
    //[SerializeField] private int _maxPoolCapacityForGunFactory;
    //[SerializeField] private int _capacityGun;

    //[Header("Bullet Factory Settings")]
    //[SerializeField] private int _initialPoolSizeForBulletFactory;
    //[SerializeField] private int _maxPoolCapacityForBulletFactory;

    private GunFactory _gunFactory;
    private BulletFactory _bulletFactory;

    protected override ModelsProduction<Truck, TruckFactory> CreateModelsProduction()
    {
        TrucksProduction production = new TrucksProduction();

        production.AddFactory<GreenTruck>(new GreenTruckFactory(_gunFactory,
                                                                _factorySettings.InitialPoolSize,
                                                                _factorySettings.MaxPoolCapacity));

        production.AddFactory<OrangeTruck>(new OrangeTruckFactory(_gunFactory,
                                                                  _factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        production.AddFactory<PurpleTruck>(new PurpleTruckFactory(_gunFactory,
                                                                  _factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        return production;
    }

    protected override Field CreateField(LevelSettings levelSettings)
    {
        return new Field(_position.position,
                        _position.forward,
                        _position.right,
                        _intervalBetweenModels,
                        _distanceBetweenModels,
                        levelSettings.WidthTrucksField,
                        levelSettings.LengthTrucksField);
    }

    protected override PresentersProduction<Truck> CreatePresentersProduction()
    {
        PresentersProduction<Truck> production = new PresentersProduction<Truck>();

        production.AddFactory<GreenTruck>(_greenTruckPresenterFactory);
        production.AddFactory<OrangeTruck>(_orangeTruckPresenterFactory);
        production.AddFactory<PurpleTruck>(_purpleTruckPresenterFactory);

        return production;
    }

    //private void CreateGunFactory()
    //{
    //    _bulletFactory = new BulletFactory(_initialPoolSizeForBulletFactory,
    //                                       _maxPoolCapacityForGunFactory);

    //    _gunFactory = new GunFactory(_initialPoolSizeForGunFactory,
    //                                 _maxPoolCapacityForGunFactory,
    //                                 _bulletFactory,
    //                                 _capacityGun);
    //}

    protected override void InitializePresenterFactories()
    {
        //CreateGunFactory();

        _greenTruckPresenterFactory.Initialize();
        _orangeTruckPresenterFactory.Initialize();
        _purpleTruckPresenterFactory.Initialize();
    }
}