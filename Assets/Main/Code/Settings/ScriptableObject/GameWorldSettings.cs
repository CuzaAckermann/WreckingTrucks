using UnityEngine;

[CreateAssetMenu(fileName = "NewGameWorldSettings", menuName = "Settings/New Game World Settings")]
public class GameWorldSettings : ScriptableObject
{
    [SerializeField] private PlaneSpaceSettings _planeSpaceSettings;
    [SerializeField] private RoadSpaceSettings _roadSpaceSettings;

    [Header("Computer Player Settings")]
    [SerializeField] private ComputerPlayerSettings _computerPlayerSettings;

    [Header("Abilities")]
    [SerializeField] private BlockFieldManipulatorSettings _blockFieldManipulatorSettings;

    [Header("Global Settings")]
    [SerializeField] private GlobalSettings _globalSettings;

    [Header("Current LevelSettings")]
    [SerializeField] private LevelSettings _levelSettings;

    [Header("Nonstop Game Settings")]
    [SerializeField] private NonstopGameSettings _nonstopGameSettings;

    public PlaneSpaceSettings PlaneSpaceSettings => _planeSpaceSettings;

    public RoadSpaceSettings RoadSpaceSettings => _roadSpaceSettings;

    public ComputerPlayerSettings ComputerPlayerSettings => _computerPlayerSettings;

    public BlockFieldManipulatorSettings BlockFieldManipulatorSettings => _blockFieldManipulatorSettings;

    public GlobalSettings GlobalSettings => _globalSettings;

    public LevelSettings LevelSettings => _levelSettings;

    public NonstopGameSettings NonstopGameSettings => _nonstopGameSettings;

    public void SetFieldTransforms(PlacementSettings placementSettings)
    {
        _globalSettings.SetFieldTransforms(placementSettings);

        _roadSpaceSettings.SetPath(placementSettings.PathForTrucks);
        _planeSpaceSettings.SetSettings(placementSettings.PlaneSlotPosition,
                                        placementSettings.PathForPlane);
    }

    public void SetLevelSettings(LevelSettings levelSettings)
    {
        _levelSettings = levelSettings;
    }
}