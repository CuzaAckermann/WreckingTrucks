using UnityEngine;

public class FieldSpace : MonoBehaviour
{
    [Header("Settings Blocks")]

    [Header("Settings BlocksFieldFiller")]
    [SerializeField] private int _startCapacityQueueForBlocksFieldFiller = 100;

    private BlocksFieldFiller _blocksFieldFiller;

    [Header("Settings BlocksField")]
    [SerializeField] private Transform _positionForBlocksField;
    [SerializeField] private Vector3 _columnDirectionForBlocksField = Vector3.forward;
    [SerializeField] private Vector3 _rowDirectionForBlocksField = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumnsForBlocksField = 10;
    [SerializeField, Min(1)] private int _capacityColumnForBlocksField = 50;
    [SerializeField] private int _spawnPositionForBlocksField = 10;

    private BlocksField _blocksField;

    [Header("Settings Stopwatch for BlocksFieldFiller")]
    [SerializeField] private float _notificationIntervalForBlocksFieldFiller = 0.5f;

    private Stopwatch _stopwatchForBlocksFieldFiller;

    [Header("Settings Trucks")]

    [Header("Settings TrucksFieldFiller")]
    [SerializeField] private int _startCapacityQueueForTrucksFieldFiller = 100;

    private TrucksFieldFiller _trucksFieldFiller;

    [Header("Settings TrucksField")]
    [SerializeField] private Transform _positionForTrucksField;
    [SerializeField] private Vector3 _columnDirectionForTrucksField = Vector3.back;
    [SerializeField] private Vector3 _rowDirectionForTrucksField = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumnsForTrucksField = 3;
    [SerializeField, Min(1)] private int _capacityColumnForTrucksField = 5;
    [SerializeField] private int _spawnPositionForTrucksField = 5;

    private TrucksField _trucksField;

    [Header("Settings Stopwatch for TrucksFieldFiller")]
    [SerializeField] private float _notificationIntervalForTrucksFieldFiller = 0.5f;

    private Stopwatch _stopwatchForTrucksFieldFiller;

    [Header("Settings Level Generation")]
    [SerializeField, Min(1)] private int _amountRows = 10;

    private LevelGenerator _levelGenerator;

    public BlocksFieldFiller BlocksFieldFiller => _blocksFieldFiller;

    public BlocksField BlocksField => _blocksField;

    public Stopwatch StopwatchForBlocksFieldFiller => _stopwatchForBlocksFieldFiller;

    public TrucksFieldFiller TrucksFieldFiller => _trucksFieldFiller;

    public TrucksField TrucksField => _trucksField;

    public Stopwatch StopwatchForTrucksFieldFiller => _stopwatchForTrucksFieldFiller;

    public void PerformFields(MoverEngine moverEngine, Productions productions)
    {
        _blocksField = new BlocksField(_positionForBlocksField.position,
                                       _columnDirectionForBlocksField,
                                       _rowDirectionForBlocksField,
                                       _amountColumnsForBlocksField,
                                       _capacityColumnForBlocksField,
                                       _spawnPositionForBlocksField,
                                       moverEngine.BlocksMover);

        _stopwatchForBlocksFieldFiller = new Stopwatch(_notificationIntervalForBlocksFieldFiller);

        _blocksFieldFiller = new BlocksFieldFiller(productions.BlocksProduction,
                                                   _blocksField,
                                                   _startCapacityQueueForBlocksFieldFiller);

        _trucksField = new TrucksField(_positionForTrucksField.position,
                                       _columnDirectionForTrucksField,
                                       _rowDirectionForTrucksField,
                                       _amountColumnsForTrucksField,
                                       _capacityColumnForTrucksField,
                                       _spawnPositionForTrucksField,
                                       moverEngine.TrucksMover);

        _stopwatchForTrucksFieldFiller = new Stopwatch(_notificationIntervalForTrucksFieldFiller);

        _trucksFieldFiller = new TrucksFieldFiller(productions.TrucksProduction,
                                                   _trucksField,
                                                   _startCapacityQueueForTrucksFieldFiller);
    }

    public void GenerateLevel()
    {
        _levelGenerator = new LevelGenerator(_blocksField.AmountColumns, _trucksField.AmountColumns);

        _levelGenerator.AddTypeBlock<GreenBlock>();
        _levelGenerator.AddTypeBlock<OrangeBlock>();
        _levelGenerator.AddTypeBlock<PurpleBlock>();

        _levelGenerator.AddTypeTruck<GreenTruck>();
        _levelGenerator.AddTypeTruck<OrangeTruck>();
        _levelGenerator.AddTypeTruck<PurpleTruck>();
    }

    public void StartLevel()
    {
        _blocksFieldFiller.PrepareBlocks(new Level(_levelGenerator.GetRowsBlocks(_amountRows)));
        _trucksFieldFiller.PrepareBlocks(new Level(_levelGenerator.GetRowsTrucks(_amountRows)));

        _stopwatchForBlocksFieldFiller.Start();
        _stopwatchForTrucksFieldFiller.Start();
    }
}