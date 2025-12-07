using System;
using System.Collections.Generic;

public abstract class RowGenerationStrategy
{
    protected readonly Random Random = new Random();

    public abstract List<ColorType> Generate(List<ColorType> differentTypes, int amountModels);

    protected void ValidateInput(List<ColorType> differentTypes, int amountModels)
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