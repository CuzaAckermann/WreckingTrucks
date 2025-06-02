using System;
using UnityEngine;

public class BlocksSpace : MonoBehaviour
{
    [Header("Settings Field")]
    [SerializeField] private Transform _position;
    [SerializeField] private Vector3 _columnDirection = Vector3.forward;
    [SerializeField] private Vector3 _rowDirection = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumns = 10;
    [SerializeField, Min(1)] private int _capacityColumn = 50;
    [SerializeField] private int _spawnPosition = 10;

    [Header("Settings FieldFiller")]
    [SerializeField] private int _startCapacityForFieldFiller = 100;

    [Header("Settings Stopwatch for FieldFiller")]
    [SerializeField] private float _notificationIntervalForFieldFiller = 0.5f;

    [Header("Settings Level Generation")]
    [SerializeField, Min(1)] private int _amountRows = 10;
    [SerializeField] private int _intervalForRowWithTwoAlternatingRandomTypesGenerator = 3;

    private BlocksField _blocksField;
    private BlocksFieldFiller _blocksFieldFiller;
    private BlocksProduction _blocksProduction;
    private PresentersProduction<Block> _blockPresentersProduction;
    private Stopwatch _stopwatchForBlocksFieldFiller;
    private Generator<Block> _blocksGenerator;
    private ITickEngineUpdaterOnlyAddAndRemove _tickEngineUpdater;

    public event Action BlocksFieldIsEmpty;

    public void Initialize(BlocksProduction blocksProduction,
                           PresentersProduction<Block> blockPresenterProduction,
                           ITickEngineUpdaterOnlyAddAndRemove tickEngineUpdater,
                           Mover<Block> blocksMover)
    {
        _blocksProduction = blocksProduction ?? throw new ArgumentNullException(nameof(blocksProduction));
        _blockPresentersProduction = blockPresenterProduction ?? throw new ArgumentNullException(nameof(blockPresenterProduction));
        _tickEngineUpdater = tickEngineUpdater ?? throw new ArgumentNullException(nameof(tickEngineUpdater));

        InitializeBlocksGenerator();
        InitializeBlocksField(blocksMover, _blocksProduction);
    }

    public void Reset()
    {
        _blocksFieldFiller.Reset();
        _blocksField.Reset();
        _stopwatchForBlocksFieldFiller.Reset();
    }

    public void Clear()
    {
        _stopwatchForBlocksFieldFiller.Stop();

        _blocksFieldFiller.Clear();
        _blocksField.Clear();

        _tickEngineUpdater.Remove(_stopwatchForBlocksFieldFiller);
    }

    public void AddRow()
    {
        _blocksFieldFiller.PrepareModels(new LevelSettings(_blocksGenerator.GetRows(1, _amountColumns)));
        _stopwatchForBlocksFieldFiller.Start();
    }

    public void PrepareFields()
    {
        _blocksFieldFiller.PrepareModels(new LevelSettings(_blocksGenerator.GetRows(_amountRows, _amountColumns)));

        _tickEngineUpdater.Add(_stopwatchForBlocksFieldFiller);
    }

    public void StartLevel()
    {
        _stopwatchForBlocksFieldFiller.Start();
    }

    private void InitializeBlocksField(Mover<Block> blocksMover, BlocksProduction blocksProduction)
    {
        _blocksField = new BlocksField(_position.position,
                                       _columnDirection,
                                       _rowDirection,
                                       _amountColumns,
                                       _capacityColumn,
                                       _spawnPosition,
                                       blocksMover);

        _stopwatchForBlocksFieldFiller = new Stopwatch(_notificationIntervalForFieldFiller);

        _blocksFieldFiller = new BlocksFieldFiller(blocksProduction,
                                                   _blocksField,
                                                   _startCapacityForFieldFiller);

        _blocksField.ModelTaken += OnBlockTaken;
        _stopwatchForBlocksFieldFiller.IntervalPassed += _blocksFieldFiller.PutModels;
        _blocksFieldFiller.FillingCompleted += OnBlocksFieldFilled;

        _blocksField.AllColumnIsEmpty += OnAllColumnIsEmpty;
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

    private void OnBlockTaken(Block block)
    {
        _blockPresentersProduction.CreatePresenter(block).Initialize(block);
    }

    private void OnBlocksFieldFilled()
    {
        _stopwatchForBlocksFieldFiller.Stop();
    }

    private void OnAllColumnIsEmpty()
    {
        BlocksFieldIsEmpty?.Invoke();
    }
}