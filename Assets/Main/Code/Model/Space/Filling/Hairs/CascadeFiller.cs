public class CascadeFiller : FillingStrategy
{
    public CascadeFiller(Stopwatch stopwatch, float frequency, SpawnDetector spawnDetector)
                  : base(stopwatch, frequency, spawnDetector)
    {

    }

    protected override void Fill(IRecordStorage recordStorage)
    {
        if (TryGetRecord(recordStorage, out RecordPlaceableModel record))
        {
            PlaceModel(record);
        }
        else
        {
            OnFillingFinished();
        }
    }
}