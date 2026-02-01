using System;

public class RowFiller<M> : FillingStrategy<M> where M :Model
{
    private readonly int _amountColumns;

    public RowFiller(float frequency,
                     SpawnDetector spawnDetector,
                     int amountColumns,
                     ModelFactory<M> modelFactory,
                     int spawnDistance)
              : base(frequency,
                     spawnDetector,
                     modelFactory,
                     spawnDistance)
    {
        _amountColumns = amountColumns > 0 ? amountColumns : throw new ArgumentOutOfRangeException(nameof(amountColumns));
    }

    protected override void Fill(IRecordStorage recordStorage)
    {
        for (int i = 0; i < _amountColumns; i++)
        {
            if (TryGetRecord(recordStorage, out RecordPlaceableModel record))
            {
                PlaceModel(record);
            }
            else
            {
                //OnFillingFinished();

                return;
            }
        }

        //if (recordStorage.Amount == 0)
        //{
        //    OnFillingFinished();
        //}
    }
}