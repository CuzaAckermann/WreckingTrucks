public class RowFiller : FillingStrategy
{
    public RowFiller(Stopwatch stopwatch, float frequency, SpawnDetector spawnDetector)
              : base(stopwatch, frequency, spawnDetector)
    {

    }

    protected override void Fill(FillingCard fillingCard)
    {
        for (int i = 0; i < fillingCard.AmountColumns && fillingCard.Amount > 0; i++)
        {
            PlaceModel(GetRecord(fillingCard));
        }

        if (fillingCard.Amount == 0)
        {
            OnFillingCardEmpty();
        }
    }
}