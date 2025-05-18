using System;
using System.Collections.Generic;

public class LevelGenerator
{
    private List<BlockType> _blocksTypes;
    private Random _random;

    public LevelGenerator()
    {
        _blocksTypes = new List<BlockType>();
        _random = new Random();
    }

    public void AddBlockType(BlockType blockType)
    {
        if (_blocksTypes.Contains(blockType) == false)
        {
            _blocksTypes.Add(blockType);
        }
    }

    public List<Row> GetRows(int amountRows, int amountColumns)
    {
        List<Row> rows = new List<Row>();

        for(int i = 0; i < amountRows; i++)
        {
            rows.Add(GetOneTypeBlockRow(amountColumns));
        }

        return rows;
    }

    private Row GetOneTypeBlockRow(int amount)
    {
        BlockType randomBlockType = _blocksTypes[_random.Next(0, _blocksTypes.Count)];
        List<BlockType> blocks = new List<BlockType>(amount);

        for (int i = 0; i < amount; i++)
        {
            blocks.Add(randomBlockType);
        }

        return new Row(blocks);
    }
}