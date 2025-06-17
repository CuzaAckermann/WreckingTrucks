using System;
using UnityEngine;
using Random = System.Random;

public class RainFiller : FillingStrategy
{
    private Random _random;
    private int _minAmountModelsAtTime;
    private int _maxAmountModelsAtTime;
    private int _rainHeight;

    public RainFiller(float frequency,
                      int minAmountModelsAtTime,
                      int maxAmountModelsAtTime,
                      int rainHeight)
               : base(frequency)
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

    protected override void Fill(FillingCard<Model> fillingCard)
    {
        int amountFillings = _random.Next(_minAmountModelsAtTime, _maxAmountModelsAtTime);

        for (int i = 0; i < amountFillings && fillingCard.Amount > 0; i++)
        {
            PlaceModel(GetRecordModelToPosition(fillingCard));
        }
    }

    protected override RecordModelToPosition<Model> GetRecordModelToPosition(FillingCard<Model> fillingCard)
    {
        return fillingCard.GetRecord(_random.Next(0, fillingCard.Amount));
    }

    protected override Vector3 GetSpawnPosition(RecordModelToPosition<Model> record)
    {
        return new Vector3(record.NumberOfColumn,
                           _rainHeight,
                           record.NumberOfRow);
    }
}