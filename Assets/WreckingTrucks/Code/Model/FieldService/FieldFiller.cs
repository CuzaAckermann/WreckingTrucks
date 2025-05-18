using System;
using System.Collections.Generic;

public class FieldFiller : IClearable
{
    private BlocksFactories _blocksFactories;
    private FieldOfBlocks _fieldOfBlocks;

    private Queue<Block> _blocks;

    public FieldFiller(BlocksFactories blocksFactory, FieldOfBlocks fieldOfBlocks, int startCapacityQueue)
    {
        _blocksFactories = blocksFactory ?? throw new ArgumentNullException(nameof(blocksFactory));
        _fieldOfBlocks = fieldOfBlocks ?? throw new ArgumentNullException(nameof(fieldOfBlocks));
        _blocks = new Queue<Block>(startCapacityQueue);
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
    }

    public void FillFieldAmountBlocks()
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

    public void Clear()
    {
        _blocks.Clear();
    }
}