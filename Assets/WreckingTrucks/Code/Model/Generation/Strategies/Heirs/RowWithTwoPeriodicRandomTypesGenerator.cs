using System;
using System.Collections.Generic;

public class RowWithTwoPeriodicRandomTypesGenerator : GenerationStrategy
{
    private int _amountSameTypeAtTime;
    private Type _firstType;
    private Type _secondType;

    public RowWithTwoPeriodicRandomTypesGenerator(int amountSameTypeAtTime)
    {
        if (amountSameTypeAtTime <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountSameTypeAtTime)} must be positive.");
        }

        _amountSameTypeAtTime = amountSameTypeAtTime;
    }

    public override List<Type> Generate(List<Type> differentTypes, int amountElements)
    {
        if (differentTypes == null)
        {
            throw new ArgumentNullException(nameof(differentTypes));
        }

        if (differentTypes.Count <= 1)
        {
            throw new InvalidOperationException($"Insufficient amount of types.");
        }

        if (amountElements <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountElements)} must be positive.");
        }

        List<Type> elements = new List<Type>(amountElements);
        GenerateTypesForRow(differentTypes);
        Type tempType = _firstType;

        while (elements.Count < amountElements)
        {
            int elementsToAdd = Math.Min(_amountSameTypeAtTime, amountElements - elements.Count);

            for (int i = 0; i < elementsToAdd; i++)
            {
                elements.Add(tempType);
            }

            tempType = tempType == _firstType ? _secondType : _firstType;
        }

        return elements;
    }

    private void GenerateTypesForRow(List<Type> differentTypes)
    {
        List<Type> tempTypes = new List<Type>(differentTypes);
        _firstType = tempTypes[Random.Next(0, tempTypes.Count)];
        tempTypes.Remove(_firstType);
        _secondType = tempTypes[Random.Next(0, tempTypes.Count)];
    }
}