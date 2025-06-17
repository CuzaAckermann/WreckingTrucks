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

    public FillingCard<Type> GetFillingCardType(int amountRows, int amountElementsInRow)
    {
        if (amountRows <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountRows)} must be positive.");
        }

        FillingCard<Type> fillingCard = new FillingCard<Type>(amountRows, amountElementsInRow);

        for (int i = 0; i < amountRows; i++)
        {
            GenerationStrategy strategy = _strategies[_random.Next(0, _strategies.Count)];

            List<Type> types = strategy.Generate(_types, amountElementsInRow);

            List<RecordModelToPosition<Type>> records = new List<RecordModelToPosition<Type>>(types.Count);

            for (int j = 0; j < types.Count; j++)
            {
                records.Add(new RecordModelToPosition<Type>(types[j], i, j));
            }
            
            fillingCard.AddRange(records);
        }

        return fillingCard;
    }

    public Type GenerateTypeModel()
    {
        return _types[_random.Next(0, _types.Count)];
    }
}