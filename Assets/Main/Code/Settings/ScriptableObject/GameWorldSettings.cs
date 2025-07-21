using UnityEngine;

[CreateAssetMenu(fileName = "NewGameWorldSettings", menuName = "Settings/New Game World Settings")]
public class GameWorldSettings : ScriptableObject
{
    [SerializeField] private BlockSpaceSettings _blockSpaceSettings;
    [SerializeField] private TruckSpaceSettings _truckSpaceSettings;
    [SerializeField] private CartrigeBoxSpaceSettings _cartrigeBoxSpaceSettings;
    [SerializeField] private RoadSpaceSettings _roadSpaceSettings;
    [SerializeField] private ShootingSpaceSettings _shootingSpaceSettings;
    [SerializeField] private SupplierSpaceSettings _supplierSpaceSettings;

    [SerializeField] private BlockTrackerCreatorSettings _blockTrackerCreatorSettings;

    public BlockSpaceSettings BlockSpaceSettings => _blockSpaceSettings;

    public TruckSpaceSettings TruckSpaceSettings => _truckSpaceSettings;

    public CartrigeBoxSpaceSettings CartrigeBoxSpaceSettings => _cartrigeBoxSpaceSettings;

    public RoadSpaceSettings RoadSpaceSettings => _roadSpaceSettings;

    public ShootingSpaceSettings ShootingSpaceSettings => _shootingSpaceSettings;

    public SupplierSpaceSettings SupplierSpaceSettings => _supplierSpaceSettings;

    public BlockTrackerCreatorSettings BlockTrackerCreatorSettings => _blockTrackerCreatorSettings;

    public void SetLevelSettings(LevelSettings levelSettings)
    {
        _blockSpaceSettings.SetFieldSettings(levelSettings.BlockFieldSettings);
        _truckSpaceSettings.SetFieldSettings(levelSettings.TruckFieldSettings);
        _cartrigeBoxSpaceSettings.SetFieldSettings(levelSettings.CartrigeBoxSettings);
    }
}