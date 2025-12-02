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

    [Header("Computer Player Settings")]
    [SerializeField] private ComputerPlayerSettings _computerPlayerSettings;

    [Header("Abilities")]
    [SerializeField] private BlockFieldManipulatorSettings _blockFieldManipulatorSettings;
    //[SerializeField] private SwapAbilitySettings _swapAbilitySettings;

    [Header("Global Entities Elements")]
    [SerializeField] private MoverSettings _moverSettings;
    [SerializeField] private RotatorSettings _rotatorSettings;

    [Header("Global Settings")]
    [SerializeField] private TruckFieldSettings _truckFieldSettings;
    [SerializeField] private CartrigeBoxFieldSettings _cartrigeBoxFieldSettings;
    [SerializeField] private ModelGeneratorSettings _modelTypeGeneratorSettings;

    public BlockSpaceSettings BlockSpaceSettings => _blockSpaceSettings;

    public TruckSpaceSettings TruckSpaceSettings => _truckSpaceSettings;

    public CartrigeBoxSpaceSettings CartrigeBoxSpaceSettings => _cartrigeBoxSpaceSettings;

    public PlaneSpaceSettings PlaneSpaceSettings => _planeSpaceSettings;

    public RoadSpaceSettings RoadSpaceSettings => _roadSpaceSettings;

    public ComputerPlayerSettings ComputerPlayerSettings => _computerPlayerSettings;

    public BlockFieldManipulatorSettings BlockFieldManipulatorSettings => _blockFieldManipulatorSettings;

    //public SwapAbilitySettings SwapAbilitySettings => _swapAbilitySettings;

    public MoverSettings MoverSettings => _moverSettings;

    public RotatorSettings RotatorSettings => _rotatorSettings;

    public ModelGeneratorSettings ModelTypeGeneratorSettings => _modelTypeGeneratorSettings;

    public void SetLevelSettings(PlacementSettings placementSettings,
                                 LevelSettings levelSettings)
    {
        _blockSpaceSettings.SetFieldSettings(placementSettings.BlockFieldPosition,
                                             levelSettings.BlockFieldSettings);
        _truckSpaceSettings.SetFieldSettings(placementSettings.TruckFieldPosition,
                                             _truckFieldSettings);

        _cartrigeBoxSpaceSettings.SetTransform(placementSettings.CartrigeBoxFieldPosition);
        _cartrigeBoxSpaceSettings.SetAmountCartrigeBox(levelSettings.AmountCartrigeBoxes);

        _roadSpaceSettings.SetPath(placementSettings.PathForTrucks);
        _planeSpaceSettings.SetSettings(placementSettings.PlaneSlotPosition,
                                                 placementSettings.PathForPlane);
    }

    public void SetNonstopGameSettings(PlacementSettings placementSettings,
                                       NonstopGameSettings nonstopGameSettings)
    {
        _blockSpaceSettings.SetFieldTransform(placementSettings.BlockFieldPosition);
        _blockSpaceSettings.SetNonstopGameBlockFieldSettings(nonstopGameSettings.BlockFieldSettings);

        _truckSpaceSettings.SetFieldSettings(placementSettings.TruckFieldPosition,
                                             nonstopGameSettings.TruckFieldSettings);
        _cartrigeBoxSpaceSettings.SetFieldSettings(placementSettings.CartrigeBoxFieldPosition,
                                                   nonstopGameSettings.CartrigeBoxSettings);
        _roadSpaceSettings.SetPath(placementSettings.PathForTrucks);
        _planeSpaceSettings.SetSettings(placementSettings.PlaneSlotPosition,
                                                 placementSettings.PathForPlane);
    }
}