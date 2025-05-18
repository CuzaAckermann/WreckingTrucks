using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Settings FieldFiller")]
    [SerializeField] private int _startCapacityQueue = 100;

    [Header("Settings FieldOfBlocks")]
    [SerializeField] private Transform _position;
    [SerializeField] private Vector3 _columnDirection = Vector3.forward;
    [SerializeField] private Vector3 _rowDirection = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumns = 10;
    [SerializeField, Min(1)] private int _capacityColumn = 50;

    [Header("Settings BlockMover")]
    [SerializeField, Min(1)] private int _capacityListWithBlocks = 300;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 5;
    [SerializeField, Min(0.001f)] private float _minDistanceToTargetPosition = 0.01f;

    [Header("Settings BlocksFactories")]
    [SerializeField, Min(1)] private int _startAmountInPool = 100;
    [SerializeField, Min(1)] private int _capacityPoolWithBlocks = 250;

    [Header("Settings BlockPresenterFactory")]
    [SerializeField] private BlockPresenterFactory _presenterFactory;

    [Header("Settings Stopwatch")]
    [SerializeField] private float _notificationInterval = 0.5f;

    [Header("Settings level generation")]
    [SerializeField] private BlockType _blockType = BlockType.Green;
    [SerializeField, Min(1)] private int _amountRows = 10;

    [Header("UI")]
    [SerializeField] private AddRowButton _addRowButton;
    [SerializeField] private ResetButton _resetButton;
    [SerializeField] private EndLevelWindow _endLevelWindow;

    private FieldFiller _fieldFiller;
    private FieldOfBlocks _fieldOfBlocks;
    private BlocksMover _blocksMover;
    private BlocksFactories _blocksFactories;
    private Stopwatch _stopwatch;
    private LevelGenerator _levelGenerator;
    private Level _testLevel;

    private List<ITickable> _tickables;
    private List<IClearable> _clearables;

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

    private void PerformInitialization()
    {
        _blocksMover = new BlocksMover(_capacityListWithBlocks, _movementSpeed, _minDistanceToTargetPosition);
        _fieldOfBlocks = new FieldOfBlocks(_position.position, _columnDirection, _rowDirection,
                                           _amountColumns, _capacityColumn, _blocksMover);
        _stopwatch = new Stopwatch(_notificationInterval);

        PrepareFactories();

        _fieldFiller = new FieldFiller(_blocksFactories, _fieldOfBlocks, _startCapacityQueue);

        PrepareLevel();
        PrepareLists();
    }

    private void PrepareFactories()
    {
        _blocksFactories = new BlocksFactories();
        _blocksFactories.RegisterFactory(BlockType.Green, new GreenBlockFactory(_startAmountInPool, _capacityPoolWithBlocks));
        _blocksFactories.RegisterFactory(BlockType.Orange, new OrangeBlockFactory(_startAmountInPool, _capacityPoolWithBlocks));
        _blocksFactories.RegisterFactory(BlockType.Purple, new PurpleBlockFactory(_startAmountInPool, _capacityPoolWithBlocks));

        _presenterFactory.Initialize();
    }

    private void PrepareLevel()
    {
        _levelGenerator = new LevelGenerator();
        _levelGenerator.AddBlockType(_blockType);
        _testLevel = new Level(_levelGenerator.GetRows(_amountRows, _fieldOfBlocks.AmountColumns));
    }

    private void PrepareLists()
    {
        _tickables = new List<ITickable>() { _blocksMover, _stopwatch };
        _clearables = new List<IClearable>() { _fieldFiller, _blocksMover, _fieldOfBlocks };
    }

    private void StartLevel()
    {
        _fieldFiller.PrepareBlocks(_testLevel);
        _endLevelWindow.HideWindow();
        _stopwatch.Start();
    }

    #region Event Callbacks
    private void OnBlockTaken(Block block)
    {
        _presenterFactory.GetPresenter().Initialize(block);
    }

    private void OnFieldFilled()
    {
        _stopwatch.Stop();
    }

    private void OnIntervalPassed()
    {
        _fieldFiller.FillFieldAmountBlocks();
    }

    private void OnAddRowButtonPressed()
    {
        _fieldFiller.PrepareBlocks(new Level(_levelGenerator.GetRows(1, _fieldOfBlocks.AmountColumns)));
        _stopwatch.Start();
    }

    private void OnResetButtonPressed()
    {
        _stopwatch.Stop();

        foreach (var clearable in _clearables)
        {
            clearable.Clear();
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
        _fieldOfBlocks.BlockTaken += OnBlockTaken;
        _fieldFiller.FillingCompleted += OnFieldFilled;
        _stopwatch.IntervalPassed += OnIntervalPassed;
    }

    private void UnsubscribeMainLogic()
    {
        _fieldOfBlocks.BlockTaken -= OnBlockTaken;
        _fieldFiller.FillingCompleted -= OnFieldFilled;
        _stopwatch.IntervalPassed -= OnIntervalPassed;
    }

    private void SubscribeUI()
    {
        _addRowButton.AddRowButtonPressed += OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed += OnResetButtonPressed;
        _fieldOfBlocks.AllColumnIsEmpty += OnAllColumnIsEmpty;
    }

    private void UnsubscribeUI()
    {
        _addRowButton.AddRowButtonPressed -= OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed -= OnResetButtonPressed;
        _fieldOfBlocks.AllColumnIsEmpty -= OnAllColumnIsEmpty;
    }
    #endregion
}