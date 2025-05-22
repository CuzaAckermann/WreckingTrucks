using System;
using System.Collections.Generic;

public class FieldFiller : IClearable, IResetable
{
    private BlocksFactories _blocksFactories;
    private FieldWithBlocks _fieldOfBlocks;

    private Queue<Block> _blocks;

    private Random _random;
    private List<Action> _fillingOptions;
    private int _numberOfCurrentColumn;
    private bool _isIncreasing = true;
    private Action _currentFillingOption;

    public FieldFiller(BlocksFactories blocksFactory, FieldWithBlocks fieldOfBlocks, int startCapacityQueue)
    {
        _blocksFactories = blocksFactory ?? throw new ArgumentNullException(nameof(blocksFactory));
        _fieldOfBlocks = fieldOfBlocks ?? throw new ArgumentNullException(nameof(fieldOfBlocks));
        _blocks = new Queue<Block>(startCapacityQueue);
        _random = new Random();
        _fillingOptions = new List<Action>();
        _numberOfCurrentColumn = 0;

        _fillingOptions.Add(FillRowOfField);
        _fillingOptions.Add(FillByZigZag);
        _fillingOptions.Add(FillByCascade);
    }

    public event Action FillingCompleted;

    public void PrepareBlocks(Level level)
    {
        for (int i = 0; i < level.Rows.Count; i++)
        {
            Row row = level.Rows[i];

            for (int j = 0; j <  row.Blocks.Count; j++)
            {
                _blocks.Enqueue(_blocksFactories.GetBlock(row.Blocks[j]));
            }
        }

        _currentFillingOption = _fillingOptions[_random.Next(0, _fillingOptions.Count)];
    }

    public void PutBlocks()
    {
        _currentFillingOption?.Invoke();
    }

    public void Reset()
    {
        _numberOfCurrentColumn = 0;
    }

    public void Clear()
    {
        _blocks.Clear();
    }

    private void FillRowOfField()
    {
        List<Block> blocks = new List<Block>(_fieldOfBlocks.AmountColumns);

        for (int i = 0; i < _fieldOfBlocks.AmountColumns; i++)
        {
            blocks.Add(_blocks.Dequeue());
        }

        _fieldOfBlocks.PlaceBlocks(blocks);

        if (_blocks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }
    }

    private void FillByZigZag()
    {
        Block block = _blocks.Dequeue();

        _fieldOfBlocks.PlaceBlock(block, _numberOfCurrentColumn);

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

            if (_numberOfCurrentColumn == _fieldOfBlocks.AmountColumns)
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

        _fieldOfBlocks.PlaceBlock(block, _numberOfCurrentColumn);

        if (_blocks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }

        GenerateNumberNextColumn();
    }

    private void GenerateNumberNextColumn()
    {
        _numberOfCurrentColumn = (_numberOfCurrentColumn + 1) % _fieldOfBlocks.AmountColumns;
    }
}