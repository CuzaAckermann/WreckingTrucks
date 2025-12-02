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
                      int minAmountModelsAtTime,
                      int maxAmountModelsAtTime,
                      int rainHeight)
               : base(stopwatch, frequency)
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

    protected override void Fill(FillingCard fillingCard)
    {
        int amountFillings = _random.Next(_minAmountModelsAtTime, _maxAmountModelsAtTime);

        for (int i = 0; i < amountFillings && fillingCard.Amount > 0; i++)
        {
            PlaceModel(GetRecord(fillingCard));
        }
    }

    protected override RecordPlaceableModel GetRecord(FillingCard fillingCard)
    {
        return fillingCard.GetRecord(_random.Next(0, fillingCard.Amount));
    }

    protected override Vector3 GetSpawnPosition(RecordPlaceableModel record)
    {
        return new Vector3(record.IndexOfColumn,
                           _rainHeight,
                           record.IndexOfRow);
    }
}