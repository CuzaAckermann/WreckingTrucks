public class RowWithFixedPeriodGenerator : RowWithSeveralTypesGenerator
{
    public RowWithFixedPeriodGenerator(int amountTypes) : base(amountTypes)
    {

    }

    protected override int CalculatePeriod(int remainingModels, int typesLeft)
    {
        return remainingModels / typesLeft;
    }
}