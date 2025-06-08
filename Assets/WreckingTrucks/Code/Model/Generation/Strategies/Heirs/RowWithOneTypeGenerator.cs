using System;
using System.Collections.Generic;

public class RowWithOneTypeGenerator : GenerationStrategy
{
    public override List<Type> Generate(List<Type> differentTypes, int amountElements)
    {
        List<Type> elements = new List<Type>(amountElements);
        Type randomTypeElement = differentTypes[Random.Next(0, differentTypes.Count)];

        for (int i = 0; i < amountElements; i++)
        {
            elements.Add(randomTypeElement);
        }

        return elements;
    }
}