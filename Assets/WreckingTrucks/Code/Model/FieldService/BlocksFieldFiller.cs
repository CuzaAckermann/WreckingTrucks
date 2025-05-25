using System;
using System.Collections.Generic;

public class BlocksFieldFiller : FieldFiller<Block>
{
    private BlocksProduction _blocksFactories;
    private BlocksField _modelsField;

    private Queue<Block> _blocks;

    private List<Action> _fillingOptions;
    private int _numberOfCurrentColumn;
    private bool _isIncreasing = true;
    private Action _currentFillingOption;

    private Random _random;

    public BlocksFieldFiller(BlocksProduction blocksFactory,
                             BlocksField blocksField,
                             int startCapacityQueue)
    {
        _blocksFactories = blocksFactory ?? throw new ArgumentNullException(nameof(blocksFactory));
        _modelsField = blocksField ?? throw new ArgumentNullException(nameof(blocksField));
        _blocks = new Queue<Block>(startCapacityQueue);
        _random = new Random();

        _fillingOptions = new List<Action>();
        _numberOfCurrentColumn = 0;

        _fillingOptions.Add(FillRowOfField);
        _fillingOptions.Add(FillByZigZag);
        _fillingOptions.Add(FillByCascade);
    }

    public event Action FillingCompleted;

    public void PutBlocks()
    {
        _currentFillingOption?.Invoke();
    }

    public override void Reset()
    {
        _numberOfCurrentColumn = 0;
    }

    public override void Clear()
    {
        _blocks.Clear();
    }

    public void PrepareBlocks(Level level)
    {
        for (int i = 0; i < level.Rows.Count; i++)
        {
            Row row = level.Rows[i];

            for (int j = 0; j < row.Blocks.Count; j++)
            {
                _blocks.Enqueue(_blocksFactories.GetBlock(row.Blocks[j]));
            }
        }

        _currentFillingOption = _fillingOptions[_random.Next(0, _fillingOptions.Count)];
    }

    private void FillRowOfField()
    {
        List<Block> blocks = new List<Block>(_modelsField.AmountColumns);

        for (int i = 0; i < _modelsField.AmountColumns; i++)
        {
            blocks.Add(_blocks.Dequeue());
        }

        _modelsField.PlaceModels(blocks);

        if (_blocks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }
    }

    private void FillByZigZag()
    {
        Block block = _blocks.Dequeue();

        _modelsField.PlaceModel(block, _numberOfCurrentColumn);

        if (_blocks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }

        GenerateNumberNextColumnZigzag();
    }

    private void GenerateNumberNextColumnZigzag()
    {
        if (_isIncreasing)
        {
            _numberOfCurrentColumn++;

            if (_numberOfCurrentColumn == _modelsField.AmountColumns)
            {
                _numberOfCurrentColumn--;
                _isIncreasing = false;
            }
        }
        else
        {
            _numberOfCurrentColumn--;

            if (_numberOfCurrentColumn < 0)
            {
                _numberOfCurrentColumn++;
                _isIncreasing = true;
            }
        }
    }

    private void FillByCascade()
    {
        Block block = _blocks.Dequeue();

        _modelsField.PlaceModel(block, _numberOfCurrentColumn);

        if (_blocks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }

        GenerateNumberNextColumn();
    }

    private void GenerateNumberNextColumn()
    {
        _numberOfCurrentColumn = (_numberOfCurrentColumn + 1) % _modelsField.AmountColumns;
    }
}