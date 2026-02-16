public class CascadeFiller<M> : FillingStrategy<M> where M : Model
{
    public CascadeFiller(float frequency,
                         SpawnDetector spawnDetector,
                         ModelFactory<M> modelFactory,
                         Placer placer,
                         int spawnDistance)
                  : base(frequency,
                         spawnDetector,
                         modelFactory,
                         placer,
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
            //OnFillingFinished();
        }
    }
}