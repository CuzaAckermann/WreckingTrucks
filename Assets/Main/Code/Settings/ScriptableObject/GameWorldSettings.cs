using UnityEngine;

[CreateAssetMenu(fileName = "NewGameWorldSettings", menuName = "Settings/New Game World Settings")]
public class GameWorldSettings : ScriptableObject
{
    [Header("Spaces")]
    [SerializeField] private BlockSpaceSettings _blockSpaceSettings;
    [SerializeField] private TruckSpaceSettings _truckSpaceSettings;
    [SerializeField] private CartrigeBoxSpaceSettings _cartrigeBoxSpaceSettings;
    [SerializeField] private RoadSpaceSettings _roadSpaceSettings;
    [SerializeField] private ShootingSpaceSettings _shootingSpaceSettings;
    [SerializeField] private SupplierSpaceSettings _supplierSpaceSettings;

    [Header("For Trucks")]
    [SerializeField] private BlockTrackerCreatorSettings _blockTrackerCreatorSettings;

    [Header("Computer Player Settings")]
    [SerializeField] private ComputerPlayerSettings _computerPlayerSettings;

    [Header("Abilities")]
    [SerializeField] private BlockFieldManipulatorSettings _blockFieldManipulatorSettings;
    [SerializeField] private SwapAbilitySettings _swapAbilitySettings;

    public BlockSpaceSettings BlockSpaceSettings => _blockSpaceSettings;

    public TruckSpaceSettings TruckSpaceSettings => _truckSpaceSettings;

    public CartrigeBoxSpaceSettings CartrigeBoxSpaceSettings => _cartrigeBoxSpaceSettings;

    public RoadSpaceSettings RoadSpaceSettings => _roadSpaceSettings;

    public ShootingSpaceSettings ShootingSpaceSettings => _shootingSpaceSettings;

    public SupplierSpaceSettings SupplierSpaceSettings => _supplierSpaceSettings;

    // убрать в TruckSpaceSettings?
    public BlockTrackerCreatorSettings BlockTrackerCreatorSettings => _blockTrackerCreatorSettings;

    public ComputerPlayerSettings ComputerPlayerSettings => _computerPlayerSettings;

    public BlockFieldManipulatorSettings BlockFieldManipulatorSettings => _blockFieldManipulatorSettings;

    public SwapAbilitySettings SwapAbilitySettings => _swapAbilitySettings;

    public void SetLevelSettings(PlacementSettings placementSettings,
                                 PathSettings pathSettings,
                                 LevelSettings levelSettings)
    {
        _blockSpaceSettings.SetFieldSettings(placementSettings.BlockField,
                                             levelSettings.BlockFieldSettings);
        _truckSpaceSettings.SetFieldSettings(placementSettings.TruckField,
                                             levelSettings.TruckFieldSettings);
        _cartrigeBoxSpaceSettings.SetFieldSettings(placementSettings.CartrigeBoxField,
                                                   levelSettings.CartrigeBoxSettings);
        _roadSpaceSettings.SetPathSettings(pathSettings);
    }
}