using UnityEngine;

public class PresenterProductionCreator : MonoBehaviour
{
    [Header("Presenter Factories")]
    [SerializeField] private BlockPresenterFactory _blockPresenterFactory;
    [SerializeField] private TruckPresenterFactory _truckPresenterFactory;
    [SerializeField] private BulletPresenterFactory _bulletPresenterFactory;
    [SerializeField] private CartrigeBoxPresenterFactory _cartrigeBoxPresenterFactory;
    [SerializeField] private PlanePresenterFactory _planePresenterFactory;

    [Header("Trigger Factory")]
    [SerializeField] private SpawnDetectorFactory _spawnDetectorFactory;

    [Header("Settings")]
    [SerializeField] private PresenterFactoriesSettings _presenterFactoriesSettings;

    public void Initialize(EventBus eventBus)
    {
        _blockPresenterFactory.Init(_presenterFactoriesSettings.BlockPresenterFactorySettings, eventBus);
        _truckPresenterFactory.Init(_presenterFactoriesSettings.TruckPresenterFactorySettings, eventBus);
        _bulletPresenterFactory.Init(_presenterFactoriesSettings.BulletPresenterFactorySettings, eventBus);
        _cartrigeBoxPresenterFactory.Init(_presenterFactoriesSettings.CartrigeBoxPresenterFactorySettings, eventBus);
        _planePresenterFactory.Init(_presenterFactoriesSettings.PlanePresenterFactorySettings, eventBus);

        _spawnDetectorFactory.Init(_presenterFactoriesSettings.SpawnDetectorFactorySettings, eventBus);
    }

    public PresenterProduction Create()
    {
        PresenterProduction presentersProduction = new PresenterProduction();

        AddPresenterFactories(presentersProduction);

        return presentersProduction;
    }

    public SpawnDetectorFactory CreateSpawnDetectorFactory()
    {
        return _spawnDetectorFactory;
    }

    private void AddPresenterFactories(PresenterProduction production)
    {
        production.AddFactory<Block>(_blockPresenterFactory);
        production.AddFactory<Truck>(_truckPresenterFactory);
        production.AddFactory<Bullet>(_bulletPresenterFactory);
        production.AddFactory<CartrigeBox>(_cartrigeBoxPresenterFactory);
        production.AddFactory<Plane>(_planePresenterFactory);
    }
}