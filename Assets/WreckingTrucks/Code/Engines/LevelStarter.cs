using UnityEngine;

public class LevelStarter : MonoBehaviour
{
    [Header("Engines")]
    [SerializeField] private Movers _movers;

    [Header("Blocks")]
    [SerializeField] private BlocksSpace _blocksSpace;

    [Header("Trucks")]
    [SerializeField] private TrucksSpace _trucksSpace;

    private Productions _productions;
    private TickEngineUpdater _tickEngineUpdater;

    public void Initialize(Productions productions, TickEngineUpdater tickEngineUpdater)
    {
        _productions = productions;
        _tickEngineUpdater = tickEngineUpdater;
        PerformInitializations();
        PrepareLevel();
    }

    private void OnEnable()
    {
        SubscribeUI();
    }

    private void OnDisable()
    {
        UnsubscribeUI();
    }

    private void PerformInitializations()
    {
        _movers.Initialize(_tickEngineUpdater);
        _blocksSpace.Initialize(_productions.BlocksProduction,
                                _productions.BlockPresentersProduction,
                                _tickEngineUpdater,
                                _movers.BlocksMover);

        _trucksSpace.Initialize(_productions.TrucksProduction,
                                _productions.TruckPresentersProduction,
                                _tickEngineUpdater,
                                _movers.TrucksMover);
    }

    public void PrepareLevel()
    {
        _movers.Prepare();
        _blocksSpace.PrepareFields();
        _trucksSpace.PrepareFields();
    }

    public void StartLevel()
    {
        _blocksSpace.StartLevel();
        _trucksSpace.StartLevel();
        _tickEngineUpdater.Continue();
    }

    #region Event Callbacks
    private void OnResetButtonPressed()
    {
        _tickEngineUpdater.Pause();

        _tickEngineUpdater.Clear();
        _movers.Clear();
        _blocksSpace.Clear();
        _trucksSpace.Clear();

        _blocksSpace.Reset();
        _trucksSpace.Reset();

        PrepareLevel();
        StartLevel();
    }

    private void OnBlocksFieldIsEmpty()
    {

    }
    #endregion

    #region UI Subscribes / Unsubscribes
    private void SubscribeUI()
    {
        _blocksSpace.BlocksFieldIsEmpty += OnBlocksFieldIsEmpty;
    }

    private void UnsubscribeUI()
    {
        _blocksSpace.BlocksFieldIsEmpty -= OnBlocksFieldIsEmpty;
    }
    #endregion
}