using UnityEngine;

[CreateAssetMenu(fileName = "ModelFactoriesSettings", menuName = "Settings/Factories/Model Factories Settings")]
public class ModelFactoriesSettings : ScriptableObject
{
    [SerializeField] private FactorySettings _blockFactorySettings;
    [SerializeField] private TruckFactorySettings _truckFactorySettings;
    [SerializeField] private PlaneFactorySettings _planeFactorySettings;
    [SerializeField] private FactorySettings _cartrigeBoxFactorySettings;
    [SerializeField] private FactorySettings _gunFactorySettings;
    [SerializeField] private FactorySettings _bulletFactorySettings;

    public FactorySettings BlockFactorySettings => _blockFactorySettings;

    public TruckFactorySettings TruckFactorySettings => _truckFactorySettings;

    public PlaneFactorySettings PlaneFactorySettings => _planeFactorySettings;

    public FactorySettings CartrigeBoxSettings => _cartrigeBoxFactorySettings;

    public FactorySettings GunFactorySettings => _gunFactorySettings;

    public FactorySettings BulletFactorySettings => _bulletFactorySettings;
}