using UnityEngine;

[CreateAssetMenu(fileName = "NewPresenterFactoriesSettings", menuName = "Settings/Factories/New Presenter Factories Settings")]
public class PresenterFactoriesSettings : ScriptableObject
{
    [Header("Block Presenter Factories")]
    [SerializeField] private PresenterFactorySettings<BlockPresenter> _blockPresenterFactorySettings;

    [Header("Truck Presenter Factories")]
    [SerializeField] private PresenterFactorySettings<TruckPresenter> _truckPresenterFactorySettings;

    [Header("Plane Presenter Factory")]
    [SerializeField] private PresenterFactorySettings<PlanePresenter> _planePresenterFactorySettings;

    [Header("Cartrige Box Presenter Factory")]
    [SerializeField] private PresenterFactorySettings<CartrigeBoxPresenter> _cartrigeBoxPresenterFactorySettings;

    [Header("Bullet Presenter Factory")]
    [SerializeField] private PresenterFactorySettings<BulletPresenter> _bulletPresenterFactorySettings;

    [Header("Trigger Block Presenter Detector Factory")]
    [SerializeField] private PresenterFactorySettings<SpawnDetector> _spawnDetectorFactorySettings;

    public PresenterFactorySettings<BlockPresenter> BlockPresenterFactorySettings => _blockPresenterFactorySettings;

    public PresenterFactorySettings<TruckPresenter> TruckPresenterFactorySettings => _truckPresenterFactorySettings;

    public PresenterFactorySettings<PlanePresenter> PlanePresenterFactorySettings => _planePresenterFactorySettings;

    public PresenterFactorySettings<CartrigeBoxPresenter> CartrigeBoxPresenterFactorySettings => _cartrigeBoxPresenterFactorySettings;
    
    public PresenterFactorySettings<BulletPresenter> BulletPresenterFactorySettings => _bulletPresenterFactorySettings;

    public PresenterFactorySettings<SpawnDetector> SpawnDetectorFactorySettings => _spawnDetectorFactorySettings;
}