using System;
using System.Collections.Generic;

public class Generator<M> where M : Model
{
    private List<GenerationStrategy> _strategies = new List<GenerationStrategy>();
    private List<Type> _types = new List<Type>();
    private Random _random = new Random();

    public void AddType<T>() where T : M
    {
        Type addedType = typeof(T);

        if (_types.Contains(addedType))
        {
            throw new InvalidOperationException($"{addedType} has already been added.");
        }

        _types.Add(addedType);
    }

    public void AddGenerator(GenerationStrategy strategy)
    {
        if (_strategies.Contains(strategy))
        {
            throw new InvalidOperationException($"{typeof(GenerationStrategy)} has already been added.");
        }

        _strategies.Add(strategy);
    }

    public List<Row> GetRows(int amountRows, int amountElementsInRow)
    {
        if (amountRows <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountRows)} must be positive.");
        }

        List<Row> rows = new List<Row>();

        for (int i = 0; i < amountRows; i++)
        {
            GenerationStrategy strategy = _strategies[_random.Next(0, _strategies.Count)];
            rows.Add(strategy.Generate(_types, amountElementsInRow));
        }

        return rows;
    }
}