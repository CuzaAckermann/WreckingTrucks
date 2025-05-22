using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    #region Settings Models
    [Header("Settings Models")]

    [Header("Settings Factory")]
    [SerializeField] private int _initialPoolSize = 100;
    [SerializeField] private int _maxPoolCapacity = 250;

    private BlocksFactories _blocksFactories;

    [Header("Settings FieldFiller")]
    [SerializeField] private int _startCapacityQueue = 100;

    private FieldFiller _fieldFiller;

    [Header("Settings FieldWithBlocks")]
    [SerializeField] private Transform _position;
    [SerializeField] private Vector3 _columnDirection = Vector3.forward;
    [SerializeField] private Vector3 _rowDirection = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumns = 10;
    [SerializeField, Min(1)] private int _capacityColumn = 50;

    private FieldWithBlocks _fieldWithBlocks;

    [Header("Settings LevelProgress")]
    [SerializeField] private float _timeToTarget = 10;

    private LevelProgress _levelProgress;

    [Header("Settings BlockMover")]
    [SerializeField, Min(1)] private int _capacityListWithBlocks = 300;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 5;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPosition = 0.001f;

    private BlockMover _blocksMover;

    [Header("Settings level generation")]
    [SerializeField, Min(1)] private int _amountRows = 10;

    private LevelGenerator _levelGenerator;

    [Header("Settings Stopwatch")]
    [SerializeField] private float _notificationInterval = 0.5f;

    private Stopwatch _stopwatch;
    #endregion

    #region Settings Presenters
    [Header("Settings Presenters")]

    [Header("Settings BlockPresentersFactories")]
    [SerializeField] private BlockPresentersFactories _blockPresentersFactories;
    [SerializeField, Min(1)] private int _startAmountInPool = 100;
    [SerializeField, Min(1)] private int _capacityPoolWithBlocks = 250;

    [Header("UI")]
    [SerializeField] private AddRowButton _addRowButton;
    [SerializeField] private int _amountRowsOneTime = 1;

    [SerializeField] private ResetButton _resetButton;
    [SerializeField] private EndLevelWindow _endLevelWindow;
    [SerializeField] private LevelProgressBar _progressBar;
    #endregion

    private List<ITickable> _tickables;
    private List<IClearable> _clearables;
    private List<IResetable> _resetables;

    #region Unity Callbacks
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

    private void Update()
    {
        foreach (var tickable in _tickables)
        {
            tickable.Tick(Time.deltaTime);
        }
    }

    private void OnDisable()
    {
        UnsubscribeMainLogic();
        UnsubscribeUI();
    }
    #endregion

    #region Level preparation
    private void PerformInitialization()
    {
        _blocksMover = new BlockMover(_capacityListWithBlocks, _movementSpeed, _minSqrDistanceToTargetPosition);
        _fieldWithBlocks = new FieldWithBlocks(_position.position, _columnDirection, _rowDirection,
                                               _amountColumns, _capacityColumn, _blocksMover);
        _stopwatch = new Stopwatch(_notificationInterval);
        _levelProgress = new LevelProgress(_fieldWithBlocks, _timeToTarget);

        _progressBar.Initialize(_levelProgress);

        PrepareFactories();

        _fieldFiller = new FieldFiller(_blocksFactories, _fieldWithBlocks, _startCapacityQueue);

        PrepareLevel();
        PrepareLists();
    }

    private void PrepareFactories()
    {
        _blocksFactories = new BlocksFactories(_initialPoolSize, _maxPoolCapacity);
        _blockPresentersFactories.Initiailize();
    }

    private void PrepareLevel()
    {
        _levelGenerator = new LevelGenerator(_fieldWithBlocks.AmountColumns);
        _levelGenerator.AddUniqueBlock(new GreenBlock());
        _levelGenerator.AddUniqueBlock(new OrangeBlock());
        _levelGenerator.AddUniqueBlock(new PurpleBlock());
    }

    private void PrepareLists()
    {
        _tickables = new List<ITickable>() { _blocksMover, _stopwatch, _levelProgress };
        _clearables = new List<IClearable>() { _fieldFiller, _blocksMover, _fieldWithBlocks };
        _resetables = new List<IResetable> { _fieldFiller, _fieldWithBlocks, _stopwatch };
    }

    private void StartLevel()
    {
        _fieldFiller.PrepareBlocks(new Level(_levelGenerator.GetRows(_amountRows)));
        _endLevelWindow.HideWindow();
        _stopwatch.Start();
    }
    #endregion

    #region Event Callbacks
    private void OnBlockTaken(Block block)
    {
        _blockPresentersFactories.GetBlockPresenter(block).Initialize(block);
    }

    private void OnFieldFilled()
    {
        _stopwatch.Stop();
    }

    private void OnIntervalPassed()
    {
        _fieldFiller.PutBlocks();
    }

    private void OnAddRowButtonPressed()
    {
        _fieldFiller.PrepareBlocks(new Level(_levelGenerator.GetRows(_amountRowsOneTime)));
        _stopwatch.Start();
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
    #endregion

    #region Subscribes / Unsubscribes
    private void SubscribeMainLogic()
    {
        _fieldWithBlocks.BlockTaken += OnBlockTaken;
        _fieldFiller.FillingCompleted += OnFieldFilled;
        _stopwatch.IntervalPassed += OnIntervalPassed;
    }

    private void UnsubscribeMainLogic()
    {
        _fieldWithBlocks.BlockTaken -= OnBlockTaken;
        _fieldFiller.FillingCompleted -= OnFieldFilled;
        _stopwatch.IntervalPassed -= OnIntervalPassed;
    }

    private void SubscribeUI()
    {
        _addRowButton.AddRowButtonPressed += OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed += OnResetButtonPressed;
        _fieldWithBlocks.AllColumnIsEmpty += OnAllColumnIsEmpty;
    }

    private void UnsubscribeUI()
    {
        _addRowButton.AddRowButtonPressed -= OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed -= OnResetButtonPressed;
        _fieldWithBlocks.AllColumnIsEmpty -= OnAllColumnIsEmpty;
    }
    #endregion
}