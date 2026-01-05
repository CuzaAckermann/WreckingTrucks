using System;

public class RowFiller : FillingStrategy
{
    private readonly int _amountColumns;

    public RowFiller(Stopwatch stopwatch,
                     float frequency,
                     SpawnDetector spawnDetector,
                     int amountColumns)
              : base(stopwatch, frequency, spawnDetector)
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
                OnFillingFinished();
                return;
            }
        }

        //if (recordStorage.Amount == 0)
        //{
        //    OnFillingFinished();
        //}
    }
}