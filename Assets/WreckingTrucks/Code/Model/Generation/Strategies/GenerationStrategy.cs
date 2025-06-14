using System;
using System.Collections.Generic;

public abstract class GenerationStrategy
{
    protected readonly Random Random = new Random();

    public abstract List<Type> Generate(List<Type> differentTypes, int amountModels);

    protected void ValidateInput(List<Type> differentTypes, int amountModels)
    {
        if (differentTypes == null)
        {
            throw new ArgumentNullException(nameof(differentTypes));
        }

        if (amountModels <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountModels));
        }
    }
}