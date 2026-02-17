using System;

public class RowWithRandomPeriodGenerator : RowWithSeveralTypesGenerator
{
    private readonly int _minPeriod;

    public RowWithRandomPeriodGenerator(int amountTypes, int minPeriod) : base(amountTypes)
    {
        if (minPeriod <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minPeriod));
        }

        _minPeriod = minPeriod;
    }

    protected override int CalculatePeriod(int remainingModels, int typesLeft)
    {
        int maxPossible = remainingModels - (typesLeft - 1) * _minPeriod;

        return Random.Next(_minPeriod, Math.Max(_minPeriod, maxPossible) + 1);
    }
}