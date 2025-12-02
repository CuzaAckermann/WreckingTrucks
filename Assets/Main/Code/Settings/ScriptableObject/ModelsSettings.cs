using UnityEngine;

[CreateAssetMenu(fileName = "ModelsSettings", menuName = "Settings/Models Settings")]
public class ModelsSettings : ScriptableObject
{
    [SerializeField] private ModelSettings _blockSettings;
    [SerializeField] private ModelSettings _cartrigeBoxSettings;
    [SerializeField] private ModelSettings _truckSettings;
    [SerializeField] private ModelSettings _planeSettings;
    [SerializeField] private ModelSettings _gunSettings;
    [SerializeField] private ModelSettings _bulletSettings;

    public ModelSettings BlockSettings => _blockSettings;

    public ModelSettings CartrigeBoxSettings => _cartrigeBoxSettings;

    public ModelSettings TruckSettings => _truckSettings;

    public ModelSettings PlaneSettings => _planeSettings;

    public ModelSettings GunSettings => _gunSettings;

    public ModelSettings BulletSettings => _bulletSettings;
}