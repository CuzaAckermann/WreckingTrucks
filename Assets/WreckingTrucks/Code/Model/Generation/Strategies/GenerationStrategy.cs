using System;
using System.Collections.Generic;

public abstract class GenerationStrategy
{
    protected readonly Random Random = new Random();

    public abstract List<Type> Generate(List<Type> types, int amountModels);
}