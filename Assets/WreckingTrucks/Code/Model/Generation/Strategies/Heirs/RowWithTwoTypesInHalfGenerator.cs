using System;
using System.Collections.Generic;
using UnityEngine;

public class RowWithTwoTypesInHalfGenerator : GenerationStrategy
{
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
        List<Type> tempTypes = new List<Type>(differentTypes);

        Type randomTypeElement = tempTypes[Random.Next(0, tempTypes.Count)];
        int halfRow = amountElements / 2;
        int i = 0;

        for (; i < halfRow; i++)
        {
            elements.Add(randomTypeElement);
        }

        tempTypes.Remove(randomTypeElement);
        randomTypeElement = tempTypes[Random.Next(0, tempTypes.Count)];

        for (; i < amountElements; i++)
        {
            elements.Add(randomTypeElement);
        }

        return elements;
    }
}