using UnityEngine;

[CreateAssetMenu(fileName = "NewGameWorldSettings", menuName = "Settings/New Game World Settings")]
public class GameWorldSettings : ScriptableObject
{
    [Header("Spaces")]
    [SerializeField] private BlockSpaceSettings _blockSpaceSettings;
    [SerializeField] private TruckSpaceSettings _truckSpaceSettings;
    [SerializeField] private CartrigeBoxSpaceSettings _cartrigeBoxSpaceSettings;
    [SerializeField] private PlaneSpaceSettings _planeSpaceSettings;

    [SerializeField] private RoadSpaceSettings _roadSpaceSettings;

    [SerializeField] private ShootingSpaceSettings _shootingSpaceSettings;
    [SerializeField] private SupplierSpaceSettings _supplierSpaceSettings;

    [Header("For Trucks")]
    [SerializeField] private BlockTrackerSettings _blockTrackerCreatorSettings;

    [Header("Computer Player Settings")]
    [SerializeField] private ComputerPlayerSettings _computerPlayerSettings;

    [Header("Abilities")]
    [SerializeField] private BlockFieldManipulatorSettings _blockFieldManipulatorSettings;
    [SerializeField] private SwapAbilitySettings _swapAbilitySettings;

    public BlockSpaceSettings BlockSpaceSettings => _blockSpaceSettings;

    public TruckSpaceSettings TruckSpaceSettings => _truckSpaceSettings;

    public CartrigeBoxSpaceSettings CartrigeBoxSpaceSettings => _cartrigeBoxSpaceSettings;

    public PlaneSpaceSettings PlaneSpaceSettings => _planeSpaceSettings;

    public RoadSpaceSettings RoadSpaceSettings => _roadSpaceSettings;

    public ShootingSpaceSettings ShootingSpaceSettings => _shootingSpaceSettings;

    public SupplierSpaceSettings SupplierSpaceSettings => _supplierSpaceSettings;

    // убрать в TruckSpaceSettings?
    public BlockTrackerSettings BlockTrackerCreatorSettings => _blockTrackerCreatorSettings;

    public ComputerPlayerSettings ComputerPlayerSettings => _computerPlayerSettings;

    public BlockFieldManipulatorSettings BlockFieldManipulatorSettings => _blockFieldManipulatorSettings;

    public SwapAbilitySettings SwapAbilitySettings => _swapAbilitySettings;

    public void SetLevelSettings(PlacementSettings placementSettings,
                                 LevelSettings levelSettings)
    {
        _blockSpaceSettings.SetFieldSettings(placementSettings.BlockFieldPosition,
                                             levelSettings.BlockFieldSettings);
        _truckSpaceSettings.SetFieldSettings(placementSettings.TruckFieldPosition,
                                             levelSettings.TruckFieldSettings);
        _cartrigeBoxSpaceSettings.SetFieldSettings(placementSettings.CartrigeBoxFieldPosition,
                                                   levelSettings.CartrigeBoxSettings);
        _roadSpaceSettings.SetPath(placementSettings.PathForTrucks);
        _planeSpaceSettings.SetSettings(placementSettings.PlaneSlotPosition,
                                                 placementSettings.PathForPlane);
    }
}