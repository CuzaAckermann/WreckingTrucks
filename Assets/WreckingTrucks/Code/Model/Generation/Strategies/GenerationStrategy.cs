using System;
using System.Collections.Generic;

public abstract class GenerationStrategy
{
    protected readonly Random Random = new Random();

    public abstract Row Generate(List<Type> types, int amountModels);
}