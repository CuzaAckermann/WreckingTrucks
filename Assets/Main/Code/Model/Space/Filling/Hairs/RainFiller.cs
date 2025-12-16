using System;
using UnityEngine;
using Random = System.Random;

public class RainFiller : FillingStrategy
{
    private readonly Random _random;
    private readonly int _minAmountModelsAtTime;
    private readonly int _maxAmountModelsAtTime;
    private readonly int _rainHeight;

    public RainFiller(Stopwatch stopwatch,
                      float frequency,
                      SpawnDetector spawnDetector,
                      int minAmountModelsAtTime,
                      int maxAmountModelsAtTime,
                      int rainHeight)
               : base(stopwatch, frequency, spawnDetector)
    {
        if (minAmountModelsAtTime <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minAmountModelsAtTime));
        }

        if (maxAmountModelsAtTime <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxAmountModelsAtTime));
        }

        if (rainHeight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rainHeight));
        }

        if (minAmountModelsAtTime >= maxAmountModelsAtTime)
        {
            throw new ArgumentOutOfRangeException(nameof(minAmountModelsAtTime));
        }

        _minAmountModelsAtTime = minAmountModelsAtTime;
        _maxAmountModelsAtTime = maxAmountModelsAtTime;
        _rainHeight = rainHeight;
        _random = new Random();
    }

    protected override void Fill(IRecordStorage recordStorage)
    {
        int amountFillings = _random.Next(_minAmountModelsAtTime, _maxAmountModelsAtTime);

        for (int i = 0; i < amountFillings; i++)
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

        if (recordStorage.Amount == 0)
        {
            OnFillingFinished();
        }
    }

    //protected override RecordPlaceableModel GetRecord(IRecordStorage recordStorage)
    //{
    //    //recordStorage.TryGetRecord(_random.Next(0, recordStorage.Amount), out RecordPlaceableModel record);
    //    /////////////////////////////////////////////
    //    return record;
    //}

    protected override Vector3 GetLocalSpawnPosition(RecordPlaceableModel record)
    {
        return new Vector3(record.IndexOfColumn,
                           _rainHeight,
                           record.IndexOfRow);
    }
}