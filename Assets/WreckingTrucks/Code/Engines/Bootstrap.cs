using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Engines")]
    [SerializeField] private FieldSpace _fieldSpace;
    [SerializeField] private Productions _productions;
    [SerializeField] private MoverEngine _moverEngine;
    [SerializeField] private TickEngineUpdater _tickEngineUpdater;

    [Header("UI")]
    [SerializeField] private AddRowButton _addRowButton;
    [SerializeField] private int _amountRowsOneTime = 1;

    [SerializeField] private ResetButton _resetButton;
    [SerializeField] private EndLevelWindow _endLevelWindow;
    
    private List<IClearable> _clearables;
    private List<IResetable> _resetables;

    private void Awake()
    {
        PerformInitialization();
    }

    private void OnEnable()
    {
        SubscribeMainLogic();
        SubscribeUI();
    }

    private void Start()
    {
        StartLevel();
    }

    private void OnDisable()
    {
        UnsubscribeMainLogic();
        UnsubscribeUI();
    }

    private void PerformInitialization()
    {
        _moverEngine.Initialize();
        _productions.Initialize();

        _fieldSpace.PerformFields(_moverEngine, _productions);
        _fieldSpace.GenerateLevel();

        PrepareTickEngine();
        PrepareLists();
    }

    private void PrepareTickEngine()
    {
        _tickEngineUpdater.Initialize();
        _tickEngineUpdater.AddTickable(_moverEngine.BlocksMover);
        _tickEngineUpdater.AddTickable(_moverEngine.TrucksMover);
        _tickEngineUpdater.AddTickable(_fieldSpace.StopwatchForBlocksFieldFiller);
        _tickEngineUpdater.AddTickable(_fieldSpace.StopwatchForTrucksFieldFiller);
        _tickEngineUpdater.Continue();
    }

    private void PrepareLists()
    {
        _clearables = new List<IClearable>()
        {
            _moverEngine.BlocksMover,
            _moverEngine.TrucksMover,
            _fieldSpace.BlocksFieldFiller,
            _fieldSpace.BlocksField,
            _fieldSpace.TrucksFieldFiller,
            _fieldSpace.TrucksField
        };

        _resetables = new List<IResetable>
        {
            _fieldSpace.BlocksFieldFiller,
            _fieldSpace.BlocksField,
            _fieldSpace.StopwatchForBlocksFieldFiller,
            _fieldSpace.TrucksFieldFiller,
            _fieldSpace.TrucksField,
            _fieldSpace.StopwatchForTrucksFieldFiller
        };
    }

    private void StartLevel()
    {
        _endLevelWindow.HideWindow();
        _fieldSpace.StartLevel();
    }
    
    private void OnBlockTaken(Block block)
    {
        _productions.BlockPresentersProduction.CreatePresenter(block).Initialize(block);
    }

    private void OnFieldFilled()
    {
        _fieldSpace.StopwatchForBlocksFieldFiller.Stop();
    }

    private void OnIntervalPassed()
    {
        _fieldSpace.BlocksFieldFiller.PutBlocks();
    }

    private void OnTruckTaken(Truck truck)
    {
        _productions.TruckPresentersProduction.CreatePresenter(truck).Initialize(truck);
    }

    private void OnFieldFilledForTruck()
    {
        _fieldSpace.StopwatchForTrucksFieldFiller.Stop();
    }

    private void OnIntervalPassedForTruck()
    {
        _fieldSpace.TrucksFieldFiller.PutTrucks();
    }

    private void OnAddRowButtonPressed()
    {
        StartLevel();
    }

    private void OnResetButtonPressed()
    {
        foreach (var clearable in _clearables)
        {
            clearable.Clear();
        }

        foreach (var resetable in _resetables)
        {
            resetable.Reset();
        }

        StartLevel();
    }

    private void OnAllColumnIsEmpty()
    {
        _endLevelWindow.ShowWindow();
    }
    
    private void SubscribeMainLogic()
    {
        _fieldSpace.BlocksField.ModelTaken += OnBlockTaken;
        _fieldSpace.BlocksFieldFiller.FillingCompleted += OnFieldFilled;
        _fieldSpace.StopwatchForBlocksFieldFiller.IntervalPassed += OnIntervalPassed;

        _fieldSpace.TrucksField.ModelTaken += OnTruckTaken;
        _fieldSpace.TrucksFieldFiller.FillingCompleted += OnFieldFilledForTruck;
        _fieldSpace.StopwatchForTrucksFieldFiller.IntervalPassed += OnIntervalPassedForTruck;
    }

    private void UnsubscribeMainLogic()
    {
        _fieldSpace.BlocksField.ModelTaken -= OnBlockTaken;
        _fieldSpace.BlocksFieldFiller.FillingCompleted -= OnFieldFilled;
        _fieldSpace.StopwatchForBlocksFieldFiller.IntervalPassed -= OnIntervalPassed;

        _fieldSpace.TrucksField.ModelTaken -= OnTruckTaken;
        _fieldSpace.TrucksFieldFiller.FillingCompleted -= OnFieldFilledForTruck;
        _fieldSpace.StopwatchForTrucksFieldFiller.IntervalPassed -= OnIntervalPassedForTruck;
    }

    private void SubscribeUI()
    {
        _addRowButton.AddRowButtonPressed += OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed += OnResetButtonPressed;
        _fieldSpace.BlocksField.AllColumnIsEmpty += OnAllColumnIsEmpty;
    }

    private void UnsubscribeUI()
    {
        _addRowButton.AddRowButtonPressed -= OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed -= OnResetButtonPressed;
        _fieldSpace.BlocksField.AllColumnIsEmpty -= OnAllColumnIsEmpty;
    }
}