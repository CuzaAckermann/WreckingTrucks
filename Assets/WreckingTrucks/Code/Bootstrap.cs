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

    [Header("Settings BlocksFactory")]
    [SerializeField, Min(1)] private int _startAmountInPool = 100;
    [SerializeField, Min(1)] private int _capacityPoolWithBlocks = 250;

    [Header("Settings BlockPresenterFactory")]
    [SerializeField] private BlockPresenterFactory _presenterFactory;

    [Header("Settings Stopwatch")]
    [SerializeField] private float _notificationInterval = 0.5f;

    [Header("Settings TestLevel")]
    [SerializeField, Min(1)] private int _amountRows = 10;

    [Header("UI")]
    [SerializeField] private AddRowButton _addRowButton;
    [SerializeField] private ResetButton _resetButton;
    [SerializeField] private EndLevelWindow _endLevelWindow;

    private FieldFiller _fieldFiller;
    private FieldOfBlocks _fieldOfBlocks;
    private BlocksMover _blocksMover;
    private BlocksFactory _blocksFactory;
    private Stopwatch _stopwatch;
    private LevelGenerator _levelGenerator;
    private Level _testLevel;

    private List<ITickable> _tickables;
    private List<IClearable> _clearables;

    private void Awake()
    {
        _blocksMover = new BlocksMover(_capacityListWithBlocks, _movementSpeed, _minDistanceToTargetPosition);
        _stopwatch = new Stopwatch(_notificationInterval);

        _fieldOfBlocks = new FieldOfBlocks(_position.position,
                                           _columnDirection,
                                           _rowDirection,
                                           _amountColumns,
                                           _capacityColumn,
                                           _blocksMover);

        _blocksFactory = new BlocksFactory(_startAmountInPool, _capacityPoolWithBlocks);
        _fieldFiller = new FieldFiller(_blocksFactory, _fieldOfBlocks, _startCapacityQueue);
        _levelGenerator = new LevelGenerator();
        _levelGenerator.AddBlockTemplates(new GreenBlock());
        _testLevel = new Level(_levelGenerator.GetRows(_amountRows, _fieldOfBlocks.AmountColumns));

        _presenterFactory.Initialize();

        _tickables = new List<ITickable>() { _blocksMover, _stopwatch };
        _clearables = new List<IClearable>() { _fieldFiller, _blocksMover, _fieldOfBlocks };
    }

    private void OnEnable()
    {
        _fieldOfBlocks.BlockTaken += OnBlockTaken;
        _fieldFiller.FieldFilled += OnFieldFilled;
        _stopwatch.IntervalPassed += _fieldFiller.FillFieldAmountBlocks;
        _addRowButton.AddRowButtonPressed += OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed += OnResetButtonPressed;
        _fieldOfBlocks.AllColumnIsEmpty += _endLevelWindow.ShowWindow;
    }

    private void Start()
    {
        PrepareLevel();
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
        _fieldOfBlocks.BlockTaken -= OnBlockTaken;
        _fieldFiller.FieldFilled -= OnFieldFilled;
        _stopwatch.IntervalPassed -= _fieldFiller.FillFieldAmountBlocks;
        _addRowButton.AddRowButtonPressed -= OnAddRowButtonPressed;
        _resetButton.ResetButtonPressed -= OnResetButtonPressed;
        _fieldOfBlocks.AllColumnIsEmpty -= _endLevelWindow.ShowWindow;
    }

    private void PrepareLevel()
    {
        _fieldFiller.GenerateBlocks(_testLevel);
        _endLevelWindow.HideWindow();
        _stopwatch.Start();
    }

    private void OnBlockTaken(Block block)
    {
        _presenterFactory.GetPresenter().Init(block);
    }

    private void OnFieldFilled()
    {
        _stopwatch.Stop();
    }

    private void OnAddRowButtonPressed()
    {
        _fieldFiller.GenerateBlocks(new Level(_levelGenerator.GetRows(1, _fieldOfBlocks.AmountColumns)));
        _stopwatch.Start();
    }

    private void OnResetButtonPressed()
    {
        _stopwatch.Stop();

        foreach (var clearable in _clearables)
        {
            clearable.Clear();
        }

        PrepareLevel();
    }
}