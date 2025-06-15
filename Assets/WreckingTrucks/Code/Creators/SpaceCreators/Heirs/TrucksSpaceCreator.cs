using UnityEngine;

public class TrucksSpaceCreator : SpaceCreator<Truck, TruckFactory>
{
    [Header("Filler Settings")]

    [Header("Row Filler Settings")]
    [SerializeField] protected float _frequencyForRowFiller = 0.5f;

    [Header("Cascade Filler Settings")]
    [SerializeField] protected float _frequencyForCascadeFiller = 0.05f;

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

    protected override void InitializePresenterFactories()
    {
        _greenTruckPresenterFactory.Initialize();
        _orangeTruckPresenterFactory.Initialize();
        _purpleTruckPresenterFactory.Initialize();
    }

    protected override void CastomizeModelsProduction(ModelsProduction<Truck, TruckFactory> production)
    {
        production.AddFactory<GreenTruck>(new GreenTruckFactory(_gunFactory,
                                                                _factorySettings.InitialPoolSize,
                                                                _factorySettings.MaxPoolCapacity));

        production.AddFactory<OrangeTruck>(new OrangeTruckFactory(_gunFactory,
                                                                  _factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        production.AddFactory<PurpleTruck>(new PurpleTruckFactory(_gunFactory,
                                                                  _factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));
    }

    protected override void CastomizePresentersProduction(PresentersProduction<Truck> production)
    {
        production.AddFactory<GreenTruck>(_greenTruckPresenterFactory);
        production.AddFactory<OrangeTruck>(_orangeTruckPresenterFactory);
        production.AddFactory<PurpleTruck>(_purpleTruckPresenterFactory);
    }

    protected override void CastomizeFiller(Filler filler)
    {
        filler.AddFillingStrategy(new RowFiller(_frequencyForRowFiller));
        filler.AddFillingStrategy(new CascadeFiller(_frequencyForCascadeFiller));
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
}