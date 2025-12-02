using System;
using System.Collections.Generic;

public class RowWithPeriodicTypesGenerator : GenerationStrategy
{
    private readonly int _periodLength;
    private readonly int _typesCount;
    private List<ColorType> _selectedTypes;

    public RowWithPeriodicTypesGenerator(int periodLength, int typesCount)
    {
        if (periodLength <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(periodLength));
        }

        if (typesCount <= 1)
        {
            throw new ArgumentOutOfRangeException(nameof(typesCount));
        }

        _periodLength = periodLength;
        _typesCount = typesCount;
    }

    public override List<ColorType> Generate(List<ColorType> differentTypes, int amountElements)
    {
        ValidateInput(differentTypes, amountElements);
        SelectRandomTypes(differentTypes);

        var elements = new List<ColorType>(amountElements);
        int typeIndex = 0;

        while (elements.Count < amountElements)
        {
            ColorType currentType = _selectedTypes[typeIndex % _typesCount];
            int elementsToAdd = Math.Min(_periodLength, amountElements - elements.Count);

            for (int i = 0; i < elementsToAdd; i++)
            {
                elements.Add(currentType);
            }

            typeIndex++;
        }

        return elements;
    }

    private void SelectRandomTypes(List<ColorType> availableTypes)
    {
        if (availableTypes.Count < _typesCount)
        {
            throw new ArgumentException($"Need at least {_typesCount} different types.");
        }

        var tempList = new List<ColorType>(availableTypes);
        _selectedTypes = new List<ColorType>(_typesCount);

        for (int i = 0; i < _typesCount; i++)
        {
            int randomIndex = Random.Next(0, tempList.Count);
            _selectedTypes.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }
}