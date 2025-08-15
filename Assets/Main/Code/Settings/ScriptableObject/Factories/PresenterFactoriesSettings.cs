using UnityEngine;

[CreateAssetMenu(fileName = "NewPresenterFactoriesSettings", menuName = "Settings/Factories/New Presenter Factories Settings")]
public class PresenterFactoriesSettings : ScriptableObject
{
    [Header("Block Presenter Factories")]
    [SerializeField] private PresenterFactorySettings<GreenBlockPresenter> _greenBlockPresenterFactorySettings;
    [SerializeField] private PresenterFactorySettings<OrangeBlockPresenter> _orangeBlockPresenterFactorySettings;
    [SerializeField] private PresenterFactorySettings<PurpleBlockPresenter> _purpleBlockPresenterFactorySettings;

    [Header("Truck Presenter Factories")]
    [SerializeField] private PresenterFactorySettings<GreenTruckPresenter> _greenTruckPresenterFactorySettings;
    [SerializeField] private PresenterFactorySettings<OrangeTruckPresenter> _orangeTruckPresenterFactorySettings;
    [SerializeField] private PresenterFactorySettings<PurpleTruckPresenter> _purpleTruckPresenterFactorySettings;

    [Header("Cartrige Box Presenter Factory")]
    [SerializeField] private PresenterFactorySettings<CartrigeBoxPresenter> _cartrigeBoxPresenterFactorySettings;

    [Header("Bullet Presenter Factory")]
    [SerializeField] private PresenterFactorySettings<BulletPresenter> _bulletPresenterFactorySettings;

    public PresenterFactorySettings<GreenBlockPresenter> GreenBlockPresenterFactorySettings => _greenBlockPresenterFactorySettings;
    
    public PresenterFactorySettings<OrangeBlockPresenter> OrangeBlockPresenterFactorySettings => _orangeBlockPresenterFactorySettings;
    
    public PresenterFactorySettings<PurpleBlockPresenter> PurpleBlockPresenterFactorySettings => _purpleBlockPresenterFactorySettings;

    public PresenterFactorySettings<GreenTruckPresenter> GreenTruckPresenterFactorySettings => _greenTruckPresenterFactorySettings;
    
    public PresenterFactorySettings<OrangeTruckPresenter> OrangeTruckPresenterFactorySettings => _orangeTruckPresenterFactorySettings;
    
    public PresenterFactorySettings<PurpleTruckPresenter> PurpleTruckPresenterFactorySettings => _purpleTruckPresenterFactorySettings;

    public PresenterFactorySettings<CartrigeBoxPresenter> CartrigeBoxPresenterFactorySettings => _cartrigeBoxPresenterFactorySettings;
    
    public PresenterFactorySettings<BulletPresenter> BulletPresenterFactorySettings => _bulletPresenterFactorySettings;
}