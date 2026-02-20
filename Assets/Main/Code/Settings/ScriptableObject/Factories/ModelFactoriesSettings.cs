using UnityEngine;

[CreateAssetMenu(fileName = "ModelFactoriesSettings", menuName = "Settings/Factories/Model Factories Settings")]
public class ModelFactoriesSettings : ScriptableObject
{
    [Header("Model Factories")]
    [SerializeField] private FactorySettings _blockFactorySettings;
    [SerializeField] private FactorySettings _truckFactorySettings;
    [SerializeField] private FactorySettings _trunkFactorySettings;
    [SerializeField] private PlaneFactorySettings _planeFactorySettings;
    [SerializeField] private FactorySettings _cartrigeBoxFactorySettings;
    [SerializeField] private GunFactorySettings _gunFactorySettings;
    [SerializeField] private FactorySettings _bulletFactorySettings;

    [Header("Gun Elements Factories")]
    [SerializeField] private FactorySettings _gunnerFactorySettings;
    [SerializeField] private FactorySettings _turretFactorySettings;
    [SerializeField] private FactorySettings _barrelFactorySettings;

    [Header("Service Factories")]
    [SerializeField] private FactorySettings _stopwatchFactorySettings;
    [SerializeField] private FactorySettings _smoothValueFollowerFactorySettings;

    public FactorySettings BlockFactorySettings => _blockFactorySettings;

    public FactorySettings TruckFactorySettings => _truckFactorySettings;

    public FactorySettings TrunkFactorySettings => _trunkFactorySettings;

    public PlaneFactorySettings PlaneFactorySettings => _planeFactorySettings;

    public FactorySettings CartrigeBoxSettings => _cartrigeBoxFactorySettings;

    public GunFactorySettings GunFactorySettings => _gunFactorySettings;

    public FactorySettings BulletFactorySettings => _bulletFactorySettings;

    public FactorySettings GunnerFactorySettings => _gunnerFactorySettings;

    public FactorySettings TurretFactorySetting => _turretFactorySettings;

    public FactorySettings BarrelFactorySettings => _barrelFactorySettings;

    public FactorySettings StopwatchFactorySettings => _stopwatchFactorySettings;

    public FactorySettings SmoothValueFollowerFactorySettings => _smoothValueFollowerFactorySettings;
}