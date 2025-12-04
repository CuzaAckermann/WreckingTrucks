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

    public void Initialize()
    {
        InitPresenterFactories();
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

    private void InitPresenterFactories()
    {
        _blockPresenterFactory.Init(_presenterFactoriesSettings.BlockPresenterFactorySettings);
        _truckPresenterFactory.Init(_presenterFactoriesSettings.TruckPresenterFactorySettings);
        _bulletPresenterFactory.Init(_presenterFactoriesSettings.BulletPresenterFactorySettings);
        _cartrigeBoxPresenterFactory.Init(_presenterFactoriesSettings.CartrigeBoxPresenterFactorySettings);
        _planePresenterFactory.Init(_presenterFactoriesSettings.PlanePresenterFactorySettings);

        _spawnDetectorFactory.Init(_presenterFactoriesSettings.SpawnDetectorFactorySettings);
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