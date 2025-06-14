using System;
using System.Collections.Generic;
using UnityEngine;

public class RowWithRandomTypesGenerator : GenerationStrategy
{
    public override List<Type> Generate(List<Type> differentTypes, int amountElements)
    {
        if (differentTypes == null)
        {
            throw new ArgumentNullException(nameof(differentTypes));
        }

        if (amountElements <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountElements)} must be positive.");
        }

        List<Type> elements = new List<Type>(amountElements);

        for (int i = 0; i < amountElements; i++)
        {
            Type randomTypeElements = differentTypes[Random.Next(0, differentTypes.Count)];
            elements.Add(randomTypeElements);
        }

        return elements;
    }
}