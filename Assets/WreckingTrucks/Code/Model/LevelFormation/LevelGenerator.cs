using System;
using System.Collections.Generic;

public class LevelGenerator
{
    private List<Block> _uniqueBlocks;
    private Random _random;
    private List<Func<Row>> _rowGenerationOptions;
    private int _amountColumns;

    public LevelGenerator(int amountColumns)
    {
        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountColumns)} must be positive");
        }

        _amountColumns = amountColumns;

        _uniqueBlocks = new List<Block>();
        _random = new Random();
        _rowGenerationOptions = new List<Func<Row>>();

        _rowGenerationOptions.Add(GenerateOneTypeBlockRow);
        _rowGenerationOptions.Add(GenerateRandomTypeBlockRow);
        _rowGenerationOptions.Add(GenerateTwoTypeBlockRow);
    }

    public void AddUniqueBlock(Block block)
    {
        if (block == null)
        {
            throw new ArgumentNullException(nameof(block));
        }

        if (ConstainsType(block) == false)
        {
            _uniqueBlocks.Add(block);
        }
    }

    public List<Row> GetRows(int amountRows)
    {
        List<Row> rows = new List<Row>();

        for(int i = 0; i < amountRows; i++)
        {
            rows.Add(_rowGenerationOptions[_random.Next(0, _rowGenerationOptions.Count)]());
        }

        return rows;
    }

    private Row GenerateOneTypeBlockRow()
    {
        List<Block> blocks = new List<Block>(_amountColumns);
        Block randomBlock = _uniqueBlocks[_random.Next(0, _uniqueBlocks.Count)];

        for (int i = 0; i < _amountColumns; i++)
        {
            blocks.Add(randomBlock);
        }

        return new Row(blocks);
    }

    private Row GenerateRandomTypeBlockRow()
    {
        List<Block> blocks = new List<Block>(_amountColumns);

        for (int i = 0; i < _amountColumns; i++)
        {
            Block randomBlock = _uniqueBlocks[_random.Next(0, _uniqueBlocks.Count)];
            blocks.Add(randomBlock);
        }

        return new Row(blocks);
    }

    private Row GenerateTwoTypeBlockRow()
    {
        List<Block> blocks = new List<Block>(_amountColumns);
        Block randomBlock = _uniqueBlocks[_random.Next(0, _uniqueBlocks.Count)];
        int halfRow = _amountColumns / 2;
        int i = 0;

        for ( ; i < halfRow; i++)
        {
            blocks.Add(randomBlock);
        }

        randomBlock = _uniqueBlocks[_random.Next(0, _uniqueBlocks.Count)];

        for (; i < _amountColumns; i++)
        {
            blocks.Add(randomBlock);
        }

        return new Row(blocks);
    }

    private bool ConstainsType(Block block)
    {
        for (int i = 0; i < _uniqueBlocks.Count; i++)
        {
            if (_uniqueBlocks[i].GetType() == block.GetType())
            {
                return true;
            }
        }

        return false;
    }
}