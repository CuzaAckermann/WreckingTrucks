using System;
using System.Collections.Generic;

public class LevelGenerator
{
    private List<Block> _blockTemplates;
    private Random _random;

    public LevelGenerator()
    {
        _blockTemplates = new List<Block>();
        _random = new Random();
    }

    public void AddBlockTemplates(Block block)
    {
        if (_blockTemplates.Contains(block) == false)
        {
            _blockTemplates.Add(block);
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
        Block randomBlock = _blockTemplates[_random.Next(0, _blockTemplates.Count)];
        List<Block> blocks = new List<Block>(amount);

        for (int i = 0; i < amount; i++)
        {
            blocks.Add(randomBlock);
        }

        return new Row(blocks);
    }
}