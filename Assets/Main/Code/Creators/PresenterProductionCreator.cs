using UnityEngine;

public class PresenterProductionCreator : MonoBehaviour
{
    [Header("Block Presenter Factories")]
    [SerializeField] private GreenBlockPresenterFactory _greenBlockPresenterFactory;
    [SerializeField] private OrangeBlockPresenterFactory _orangeBlockPresenterFactory;
    [SerializeField] private PurpleBlockPresenterFactory _purpleBlockPresenterFactory;

    [Header("Truck Presenter Factories")]
    [SerializeField] private GreenTruckPresenterFactory _greenTruckPresenterFactory;
    [SerializeField] private OrangeTruckPresenterFactory _orangeTruckPresenterFactory;
    [SerializeField] private PurpleTruckPresenterFactory _purpleTruckPresenterFactory;

    [Header("Gun Presenter Factories")]
    [SerializeField] private GunPresenterFactory _gunPresenterFactory;

    [Header("Bullet Presenter Factories")]
    [SerializeField] private BulletPresenterFactory _bulletPresenterFactory;

    [Header("Cartrige Presenter Factories")]
    [SerializeField] private CartrigeBoxPresenterFactory _cartrigeBoxPresenterFactory;

    [Header("Settings")]
    [SerializeField] private PresenterFactoriesSettings _presenterFactoriesSettings;

    public void Initialize()
    {
        InitializeBlockPresenterFactories();
        InitializeTruckPresenterFactories();
        InitializeCartrigeBoxPresenterFactories();
        InitializeGunPresenterFactories();
        InitializeBulletPresenterFactories();
    }

    public PresenterProduction Create()
    {
        PresenterProduction presentersProduction = new PresenterProduction();

        AddBlockPresentersProduction(presentersProduction);
        AddTruckPresenterProduction(presentersProduction);
        AddGunPresenterProduction(presentersProduction);
        AddBulletPresenterProduction(presentersProduction);
        AddCartrigeBoxPresenterProduction(presentersProduction);

        return presentersProduction;
    }

    private void InitializeBlockPresenterFactories()
    {
        _greenBlockPresenterFactory.Initialize(_presenterFactoriesSettings.GreenBlockPresenterFactorySettings);
        _orangeBlockPresenterFactory.Initialize(_presenterFactoriesSettings.OrangeBlockPresenterFactorySettings);
        _purpleBlockPresenterFactory.Initialize(_presenterFactoriesSettings.PurpleBlockPresenterFactorySettings);
    }

    private void InitializeTruckPresenterFactories()
    {
        _greenTruckPresenterFactory.Initialize(_presenterFactoriesSettings.GreenTruckPresenterFactorySettings);
        _orangeTruckPresenterFactory.Initialize(_presenterFactoriesSettings.OrangeTruckPresenterFactorySettings);
        _purpleTruckPresenterFactory.Initialize(_presenterFactoriesSettings.PurpleTruckPresenterFactorySettings);
    }

    private void InitializeGunPresenterFactories()
    {
        _gunPresenterFactory.Initialize(_presenterFactoriesSettings.GunPresenterFactorySettings);
    }

    private void InitializeBulletPresenterFactories()
    {
        _bulletPresenterFactory.Initialize(_presenterFactoriesSettings.BulletPresenterFactorySettings);
    }

    private void InitializeCartrigeBoxPresenterFactories()
    {
        _cartrigeBoxPresenterFactory.Initialize(_presenterFactoriesSettings.CartrigeBoxPresenterFactorySettings);
    }

    private void AddBlockPresentersProduction(PresenterProduction production)
    {
        production.AddFactory<GreenBlock>(_greenBlockPresenterFactory);
        production.AddFactory<OrangeBlock>(_orangeBlockPresenterFactory);
        production.AddFactory<PurpleBlock>(_purpleBlockPresenterFactory);
    }

    private void AddTruckPresenterProduction(PresenterProduction production)
    {
        production.AddFactory<GreenTruck>(_greenTruckPresenterFactory);
        production.AddFactory<OrangeTruck>(_orangeTruckPresenterFactory);
        production.AddFactory<PurpleTruck>(_purpleTruckPresenterFactory);
    }

    private void AddGunPresenterProduction(PresenterProduction production)
    {
        production.AddFactory<Gun>(_gunPresenterFactory);
    }

    private void AddBulletPresenterProduction(PresenterProduction production)
    {
        production.AddFactory<Bullet>(_bulletPresenterFactory);
    }

    private void AddCartrigeBoxPresenterProduction(PresenterProduction production)
    {
        production.AddFactory<CartrigeBox>(_cartrigeBoxPresenterFactory);
    }
}