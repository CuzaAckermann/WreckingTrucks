using UnityEngine;

public class Starter : MonoBehaviour
{
    [Header("Engines")]
    [SerializeField] private FieldsSpace _fieldsSpace;
    [SerializeField] private Movers _movers;
    [SerializeField] private TickEngineUpdater _tickEngineUpdater;

    [Header("UI")]
    [SerializeField] private PauseButton _pauseButton;
    [SerializeField] private AddRowButton _addRowButton;
    [SerializeField] private ResetButton _resetButton;
    [SerializeField] private EndLevelWindow _endLevelWindow;

    private void Awake()
    {
        PerformInitializations();
        PrepareLevel();
    }

    private void OnEnable()
    {
        SubscribeUI();
    }

    private void Start()
    {
        StartLevel();
    }

    private void OnDisable()
    {
        UnsubscribeUI();
    }

    private void PerformInitializations()
    {
        _tickEngineUpdater.Initialize();
        _movers.Initialize(_tickEngineUpdater);
        _fieldsSpace.Initialize(_tickEngineUpdater, _movers);
    }

    private void PrepareLevel()
    {
        _movers.Prepare();
        _fieldsSpace.PrepareFields();
    }

    private void StartLevel()
    {
        _endLevelWindow.HideWindow();
        _fieldsSpace.StartLevel();
        _tickEngineUpdater.Continue();
    }

    #region Event Callbacks
    private void OnPauseButtonPressed()
    {
        _tickEngineUpdater.Switch();
    }

    private void OnAddRowButtonPressed()
    {
        _fieldsSpace.AddRow();
    }

    private void OnResetButtonPressed()
    {
        _tickEngineUpdater.Pause();

        _tickEngineUpdater.Clear();
        _movers.Clear();
        _fieldsSpace.Clear();

        _fieldsSpace.Reset();

        PrepareLevel();
        StartLevel();
    }

    private void OnBlocksFieldIsEmpty()
    {
        _endLevelWindow.ShowWindow();
    }
    #endregion

    #region UI Subscribes / Unsubscribes
    private void SubscribeUI()
    {
        _pauseButton.PauseButtonPressed += OnPauseButtonPressed;
        _addRowButton.AddRowButtonPressed += OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed += OnResetButtonPressed;
        _fieldsSpace.BlocksFieldIsEmpty += OnBlocksFieldIsEmpty;
    }

    private void UnsubscribeUI()
    {
        _pauseButton.PauseButtonPressed -= OnPauseButtonPressed;
        _addRowButton.AddRowButtonPressed -= OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed -= OnResetButtonPressed;
        _fieldsSpace.BlocksFieldIsEmpty -= OnBlocksFieldIsEmpty;
    }
    #endregion
}