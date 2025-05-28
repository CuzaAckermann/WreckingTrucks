using System;
using System.Collections.Generic;

public class BlocksGenerator : ModelGenerator<Block>
{
    private List<Type> _typeBlocks;
    private Random _random;
    private List<Func<Row>> _rowGenerationOptions;
    private int _amountColumns;

    public BlocksGenerator(int amountColumns) : base (amountColumns)
    {
        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountColumns)} must be positive");
        }

        _amountColumns = amountColumns;

        _typeBlocks = new List<Type>();
        _random = new Random();
        _rowGenerationOptions = new List<Func<Row>>();

        _rowGenerationOptions.Add(GenerateOneTypeBlockRow);
        _rowGenerationOptions.Add(GenerateRandomTypeBlockRow);
        _rowGenerationOptions.Add(GenerateTwoTypeBlockRow);
    }

    public void AddTypeBlock<T>() where T : Block
    {
        if (_typeBlocks.Contains(typeof(T)) == false)
        {
            _typeBlocks.Add(typeof(T));
        }
    }

    public List<Row> GetRows(int amountRows)
    {
        List<Row> rows = new List<Row>();

        for (int i = 0; i < amountRows; i++)
        {
            rows.Add(_rowGenerationOptions[_random.Next(0, _rowGenerationOptions.Count)]());
        }

        return rows;
    }

    private Row GenerateOneTypeBlockRow()
    {
        List<Type> blocks = new List<Type>(_amountColumns);
        Type randomTypeBlock = _typeBlocks[_random.Next(0, _typeBlocks.Count)];

        for (int i = 0; i < _amountColumns; i++)
        {
            blocks.Add(randomTypeBlock);
        }

        return new Row(blocks);
    }

    private Row GenerateRandomTypeBlockRow()
    {
        List<Type> blocks = new List<Type>(_amountColumns);

        for (int i = 0; i < _amountColumns; i++)
        {
            Type randomTypeBlock = _typeBlocks[_random.Next(0, _typeBlocks.Count)];
            blocks.Add(randomTypeBlock);
        }

        return new Row(blocks);
    }

    private Row GenerateTwoTypeBlockRow()
    {
        List<Type> blocks = new List<Type>(_amountColumns);
        Type randomTypeBlock = _typeBlocks[_random.Next(0, _typeBlocks.Count)];
        int halfRow = _amountColumns / 2;
        int i = 0;

        for (; i < halfRow; i++)
        {
            blocks.Add(randomTypeBlock);
        }

        randomTypeBlock = _typeBlocks[_random.Next(0, _typeBlocks.Count)];

        for (; i < _amountColumns; i++)
        {
            blocks.Add(randomTypeBlock);
        }

        return new Row(blocks);
    }
}