using System;
using System.Collections.Generic;

public class FieldFiller
{
    private BlocksFactory _blocksFactory;
    private FieldOfBlocks _fieldOfBlocks;

    private Queue<Block> _blocks;

    public FieldFiller(BlocksFactory blocksFactory, FieldOfBlocks fieldOfBlocks)
    {
        _blocksFactory = blocksFactory ?? throw new ArgumentNullException(nameof(blocksFactory));
        _fieldOfBlocks = fieldOfBlocks ?? throw new ArgumentNullException(nameof(fieldOfBlocks));
    }

    public event Action FieldFilled;

    public void GenerateBlocks(Level level)
    {
        _blocks = new Queue<Block>(_fieldOfBlocks.AmountColumns * level.AmountRows);

        for (int i = 0; i < level.AmountRows; i++)
        {
            for (int j = 0; j < _fieldOfBlocks.AmountColumns; j++)
            {
                _blocks.Enqueue(_blocksFactory.GetBlock());
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
            FieldFilled?.Invoke();
        }
    }
}