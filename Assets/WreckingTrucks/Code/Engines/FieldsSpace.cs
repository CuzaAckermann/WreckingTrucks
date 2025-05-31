using System;
using UnityEngine;

public class FieldsSpace : MonoBehaviour
{
    [Header("Settings Blocks")]

    [Header("Settings BlocksFieldFiller")]
    [SerializeField] private int _startCapacityQueueForBlocksFieldFiller = 100;

    [Header("Settings BlocksField")]
    [SerializeField] private Transform _positionForBlocksField;
    [SerializeField] private Vector3 _columnDirectionForBlocksField = Vector3.forward;
    [SerializeField] private Vector3 _rowDirectionForBlocksField = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumnsForBlocksField = 10;
    [SerializeField, Min(1)] private int _capacityColumnForBlocksField = 50;
    [SerializeField] private int _spawnPositionForBlocksField = 10;

    [Header("Settings Stopwatch for BlocksFieldFiller")]
    [SerializeField] private float _notificationIntervalForBlocksFieldFiller = 0.5f;

    [Header("Settings Trucks")]

    [Header("Settings TrucksFieldFiller")]
    [SerializeField] private int _startCapacityQueueForTrucksFieldFiller = 100;

    [Header("Settings TrucksField")]
    [SerializeField] private Transform _positionForTrucksField;
    [SerializeField] private Vector3 _columnDirectionForTrucksField = Vector3.back;
    [SerializeField] private Vector3 _rowDirectionForTrucksField = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumnsForTrucksField = 3;
    [SerializeField, Min(1)] private int _capacityColumnForTrucksField = 5;
    [SerializeField] private int _spawnPositionForTrucksField = 5;

    [Header("Settings Stopwatch for TrucksFieldFiller")]
    [SerializeField] private float _notificationIntervalForTrucksFieldFiller = 0.5f;

    [Header("Settings Level Generation")]
    [SerializeField, Min(1)] private int _amountRowsForBlocks = 10;
    [SerializeField, Min(1)] private int _amountRowsForTrucks = 3;

    [Header("Generators")]
    [SerializeField] private int _intervalForRowWithTwoAlternatingRandomTypesGenerator = 3;

    [Header("Productions")]
    [SerializeField] private Productions _productions;

    private BlocksFieldFiller _blocksFieldFiller;
    private BlocksField _blocksField;
    private Stopwatch _stopwatchForBlocksFieldFiller;

    private TrucksFieldFiller _trucksFieldFiller;
    private TrucksField _trucksField;
    private Stopwatch _stopwatchForTrucksFieldFiller;

    private Generator<Block> _blocksGenerator;
    private Generator<Truck> _truckGenerator;

    private ITickEngineUpdaterOnlyAddAndRemove _tickEngineUpdater;

    public event Action BlocksFieldIsEmpty;

    public void Initialize(ITickEngineUpdaterOnlyAddAndRemove tickEngineUpdater, Movers moverEngine)
    {
        _tickEngineUpdater = tickEngineUpdater ?? throw new ArgumentNullException(nameof(tickEngineUpdater));

        _productions.Initialize();
        InitializeGenerations();
        InitializeFields(moverEngine);
    }

    public void Reset()
    {
        _blocksFieldFiller.Reset();
        _blocksField.Reset();
        _stopwatchForBlocksFieldFiller.Reset();

        _trucksFieldFiller.Reset();
        _trucksField.Reset();
        _stopwatchForTrucksFieldFiller.Reset();
    }

    public void Clear()
    {
        _stopwatchForBlocksFieldFiller.Stop();
        _stopwatchForTrucksFieldFiller.Stop();

        _blocksFieldFiller.Clear();
        _blocksField.Clear();

        _trucksFieldFiller.Clear();
        _trucksField.Clear();

        _tickEngineUpdater.Remove(_stopwatchForBlocksFieldFiller);
        _tickEngineUpdater.Remove(_stopwatchForTrucksFieldFiller);
    }

    public void AddRow()
    {
        _blocksFieldFiller.PrepareModels(new Level(_blocksGenerator.GetRows(1, _amountColumnsForBlocksField)));
        _stopwatchForBlocksFieldFiller.Start();
    }

    public void PrepareFields()
    {
        _blocksFieldFiller.PrepareModels(new Level(_blocksGenerator.GetRows(_amountRowsForBlocks, _amountColumnsForBlocksField)));
        _trucksFieldFiller.PrepareModels(new Level(_truckGenerator.GetRows(_amountRowsForTrucks, _amountColumnsForTrucksField)));

        _tickEngineUpdater.Add(_stopwatchForBlocksFieldFiller);
        _tickEngineUpdater.Add(_stopwatchForTrucksFieldFiller);
    }

    public void StartLevel()
    {
        _stopwatchForBlocksFieldFiller.Start();
        _stopwatchForTrucksFieldFiller.Start();
    }

    private void InitializeFields(Movers moverEngine)
    {
        InitializeBlocksField(moverEngine.BlocksMover, _productions.BlocksProduction);
        InitializeTrucksField(moverEngine.TrucksMover, _productions.TrucksProduction);
    }

    private void InitializeBlocksField(Mover<Block> blocksMover, BlocksProduction blocksProduction)
    {
        _blocksField = new BlocksField(_positionForBlocksField.position,
                                       _columnDirectionForBlocksField,
                                       _rowDirectionForBlocksField,
                                       _amountColumnsForBlocksField,
                                       _capacityColumnForBlocksField,
                                       _spawnPositionForBlocksField,
                                       blocksMover);

        _stopwatchForBlocksFieldFiller = new Stopwatch(_notificationIntervalForBlocksFieldFiller);

        _blocksFieldFiller = new BlocksFieldFiller(blocksProduction,
                                                   _blocksField,
                                                   _startCapacityQueueForBlocksFieldFiller);

        _blocksField.ModelTaken += OnBlockTaken;
        _stopwatchForBlocksFieldFiller.IntervalPassed += _blocksFieldFiller.PutModels;
        _blocksFieldFiller.FillingCompleted += OnBlocksFieldFilled;
        _blocksField.AllColumnIsEmpty += OnAllColumnIsEmpty;
    }

    private void InitializeTrucksField(Mover<Truck> trucksMover, TrucksProduction trucksProduction)
    {
        _trucksField = new TrucksField(_positionForTrucksField.position,
                                       _columnDirectionForTrucksField,
                                       _rowDirectionForTrucksField,
                                       _amountColumnsForTrucksField,
                                       _capacityColumnForTrucksField,
                                       _spawnPositionForTrucksField,
                                       trucksMover);

        _stopwatchForTrucksFieldFiller = new Stopwatch(_notificationIntervalForTrucksFieldFiller);

        _trucksFieldFiller = new TrucksFieldFiller(trucksProduction,
                                                   _trucksField,
                                                   _startCapacityQueueForTrucksFieldFiller);

        _trucksField.ModelTaken += OnTruckTaken;
        _stopwatchForTrucksFieldFiller.IntervalPassed += _trucksFieldFiller.PutModels;
        _trucksFieldFiller.FillingCompleted += OnTrucksFieldFilled;
    }

    private void InitializeGenerations()
    {
        InitializeBlocksGenerator();
        InitializeTrucksGenerator();
    }

    private void InitializeBlocksGenerator()
    {
        _blocksGenerator = new Generator<Block>();

        _blocksGenerator.AddType<GreenBlock>();
        _blocksGenerator.AddType<OrangeBlock>();
        _blocksGenerator.AddType<PurpleBlock>();

        _blocksGenerator.AddGenerator(new RowWithOneTypeGenerator());
        _blocksGenerator.AddGenerator(new RowWithTwoTypesWithRandomMiddleGenerator());
        _blocksGenerator.AddGenerator(new RowWithTwoTypesInHalfGenerator());
        _blocksGenerator.AddGenerator(new RowWithTwoAlternatingRandomTypesGenerator(_intervalForRowWithTwoAlternatingRandomTypesGenerator));
    }

    private void InitializeTrucksGenerator()
    {
        _truckGenerator = new Generator<Truck>();

        _truckGenerator.AddType<GreenTruck>();
        _truckGenerator.AddType<OrangeTruck>();
        _truckGenerator.AddType<PurpleTruck>();

        _truckGenerator.AddGenerator(new RowWithRandomTypesGenerator());
    }

    private void OnBlockTaken(Block block)
    {
        _productions.BlockPresentersProduction.CreatePresenter(block).Initialize(block);
    }

    private void OnBlocksFieldFilled()
    {
        _stopwatchForBlocksFieldFiller.Stop();
    }

    private void OnTruckTaken(Truck truck)
    {
        _productions.TruckPresentersProduction.CreatePresenter(truck).Initialize(truck);
    }

    private void OnTrucksFieldFilled()
    {
        _stopwatchForTrucksFieldFiller.Stop();
    }

    private void OnAllColumnIsEmpty()
    {
        BlocksFieldIsEmpty?.Invoke();
    }
}