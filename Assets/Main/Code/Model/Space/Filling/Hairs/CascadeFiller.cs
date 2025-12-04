public class CascadeFiller : FillingStrategy
{
    public CascadeFiller(Stopwatch stopwatch, float frequency, SpawnDetector spawnDetector)
                  : base(stopwatch, frequency, spawnDetector)
    {

    }

    protected override void Fill(FillingCard fillingCard)
    {
        PlaceModel(GetRecord(fillingCard));

        if (fillingCard.Amount == 0)
        {
            OnFillingCardEmpty();
        }
    }
}