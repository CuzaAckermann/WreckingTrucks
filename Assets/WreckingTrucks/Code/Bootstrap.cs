using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [Header("Settings FieldOfBlocks")]
    [SerializeField] private Transform _position;
    [SerializeField] private Vector3 _columnDirection = Vector3.forward;
    [SerializeField] private Vector3 _rowDirection = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumns = 10;
    [SerializeField, Min(1)] private int _capacityColumn = 50;

    [Header("Settings BlockMover")]
    [SerializeField, Min(1)] private int _capacityHashSetWithBlocks = 300;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 5;
    [SerializeField, Min(0.001f)] private float _minDistanceToTargetPosition = 0.01f;

    [Header("Settings BlocksFactory")]
    [SerializeField, Min(1)] private int _startAmountInPool = 250;
    [SerializeField, Min(1)] private int _capacitycapacityListWithBlocks = 500;

    [Header("Settings BlockPresenterFactory")]
    [SerializeField] private PresenterBlockFactory _presenterFactory;

    [Header("Settings Stopwatch")]
    [SerializeField] private float _notificationInterval = 0.5f;

    [Header("Settings TestLevel")]
    [SerializeField, Min(1)] private int _amountRows = 10;

    [Header("Settings EndLevelWindow")]
    [SerializeField] private EndLevelWindow _endLevelWindow;

    private FieldFiller _fieldFiller;
    private FieldOfBlocks _fieldOfBlocks;
    private BlocksMover _blocksMover;
    private BlocksFactory _blocksFactory;
    private Stopwatch _stopwatch;

    private List<ITickable> _tickables;

    private void Awake()
    {
        _blocksMover = new BlocksMover(_capacityHashSetWithBlocks, _movementSpeed, _minDistanceToTargetPosition);
        _stopwatch = new Stopwatch(_notificationInterval);

        _fieldOfBlocks = new FieldOfBlocks(_position.position,
                                           _columnDirection,
                                           _rowDirection,
                                           _amountColumns,
                                           _capacityColumn,
                                           _blocksMover);

        _blocksFactory = new BlocksFactory(_startAmountInPool, _capacitycapacityListWithBlocks);
        _fieldFiller = new FieldFiller(_blocksFactory, _fieldOfBlocks);

        _presenterFactory.Init();

        _tickables = new List<ITickable>() { _blocksMover, _stopwatch };
    }

    private void OnEnable()
    {
        _fieldOfBlocks.BlockTaken += OnBlockTaken;
        _fieldFiller.FieldFilled += OnFieldFilled;
        _stopwatch.IntervalPassed += _fieldFiller.FillFieldAmountBlocks;
        _fieldOfBlocks.AllColumnIsEmpty += _endLevelWindow.ShowWindow;
    }

    private void Start()
    {
        _fieldFiller.GenerateBlocks(new Level(_amountRows));
        _endLevelWindow.HideWindow();
        _stopwatch.Start();
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
        _fieldOfBlocks.AllColumnIsEmpty -= _endLevelWindow.ShowWindow;
    }

    private void OnBlockTaken(Block block)
    {
        _presenterFactory.GetBlockPresenter().Init(block);
    }

    private void OnFieldFilled()
    {
        _stopwatch.Stop();
    }
}