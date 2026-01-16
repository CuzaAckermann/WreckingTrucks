public class CascadeFiller<M> : FillingStrategy<M> where M : Model
{
    public CascadeFiller(Stopwatch stopwatch,
                         float frequency,
                         SpawnDetector spawnDetector,
                         ModelFactory<M> modelFactory,
                         int spawnDistance)
                  : base(stopwatch,
                         frequency,
                         spawnDetector,
                         modelFactory,
                         spawnDistance)
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