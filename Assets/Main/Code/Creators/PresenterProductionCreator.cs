using UnityEngine;

public class PresenterProductionCreator : MonoBehaviour
{
    [Header("Presenter Factories")]
    [SerializeField] private BlockPresenterFactory _blockPresenterFactory;
    [SerializeField] private TruckPresenterFactory _truckPresenterFactory;
    [SerializeField] private BulletPresenterFactory _bulletPresenterFactory;
    [SerializeField] private CartrigeBoxPresenterFactory _cartrigeBoxPresenterFactory;
    [SerializeField] private PlanePresenterFactory _planePresenterFactory;

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

    private void InitPresenterFactories()
    {
        _blockPresenterFactory.Initialize(_presenterFactoriesSettings.BlockPresenterFactorySettings);
        _truckPresenterFactory.Initialize(_presenterFactoriesSettings.TruckPresenterFactorySettings);
        _bulletPresenterFactory.Initialize(_presenterFactoriesSettings.BulletPresenterFactorySettings);
        _cartrigeBoxPresenterFactory.Initialize(_presenterFactoriesSettings.CartrigeBoxPresenterFactorySettings);
        _planePresenterFactory.Initialize(_presenterFactoriesSettings.PlanePresenterFactorySettings);
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