using System;
using System.Collections.Generic;
using UnityEngine;

public class RowWithRandomTypesGenerator : GenerationStrategy
{
    public override List<ColorType> Generate(List<ColorType> differentTypes, int amountElements)
    {
        if (differentTypes == null)
        {
            throw new ArgumentNullException(nameof(differentTypes));
        }

        if (amountElements <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amountElements)} must be positive.");
        }

        List<ColorType> elements = new List<ColorType>(amountElements);

        for (int i = 0; i < amountElements; i++)
        {
            ColorType randomTypeElements = differentTypes[Random.Next(0, differentTypes.Count)];
            elements.Add(randomTypeElements);
        }

        return elements;
    }
}