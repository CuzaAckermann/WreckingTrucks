using System;
using System.Collections.Generic;

public class ModelGenerator<M> where M : Model
{
    private List<Type> _typeModels;
    private Random _random;
    private List<Func<Row>> _rowGenerationOptions;
    private int _amountColumns;

    public ModelGenerator(int amountColumns)
    {
        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountColumns)} must be positive");
        }

        _amountColumns = amountColumns;

        _typeModels = new List<Type>();
        _random = new Random();
        _rowGenerationOptions = new List<Func<Row>>();

        _rowGenerationOptions.Add(GenerateOneTypeModelRow);
        _rowGenerationOptions.Add(GenerateRandomTypeModelRow);
        _rowGenerationOptions.Add(GenerateTwoTypeModelRow);
    }

    public void AddTypeModel<T>() where T : M
    {
        if (_typeModels.Contains(typeof(T)) == false)
        {
            _typeModels.Add(typeof(T));
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

    private Row GenerateOneTypeModelRow()
    {
        List<Type> models = new List<Type>(_amountColumns);
        Type randomTypeModel = _typeModels[_random.Next(0, _typeModels.Count)];

        for (int i = 0; i < _amountColumns; i++)
        {
            models.Add(randomTypeModel);
        }

        return new Row(models);
    }

    private Row GenerateRandomTypeModelRow()
    {
        List<Type> models = new List<Type>(_amountColumns);

        for (int i = 0; i < _amountColumns; i++)
        {
            Type randomTypeBlock = _typeModels[_random.Next(0, _typeModels.Count)];
            models.Add(randomTypeBlock);
        }

        return new Row(models);
    }

    private Row GenerateTwoTypeModelRow()
    {
        List<Type> models = new List<Type>(_amountColumns);
        Type randomTypeBlock = _typeModels[_random.Next(0, _typeModels.Count)];
        int halfRow = _amountColumns / 2;
        int i = 0;

        for (; i < halfRow; i++)
        {
            models.Add(randomTypeBlock);
        }

        randomTypeBlock = _typeModels[_random.Next(0, _typeModels.Count)];

        for (; i < _amountColumns; i++)
        {
            models.Add(randomTypeBlock);
        }

        return new Row(models);
    }
}