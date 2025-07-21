using System;
using UnityEngine;
using Random = System.Random;

public class SwapFillerTEMP /* : FillingStrategy */
{
    //private Random _random;
    //private int _rainHeight;

    //public SwapFillerTEMP(float frequency,
    //                  int rainHeight)
    //           : base(frequency)
    //{
    //    if (rainHeight <= 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(rainHeight));
    //    }

    //    _rainHeight = rainHeight;
    //    _random = new Random();
    //}

    //protected override void Fill(FillingCard<Model> fillingCard)
    //{
    //    PlaceModel(GetRecordModelToPosition(fillingCard));
    //}

    //protected override RecordModelToPosition<Model> GetRecordModelToPosition(FillingCard<Model> fillingCard)
    //{
    //    return fillingCard.GetRecord(_random.Next(0, fillingCard.Amount));
    //}

    //protected override Vector3 GetSpawnPosition(RecordModelToPosition<Model> record)
    //{
    //    return new Vector3(record.NumberOfColumn,
    //                       _rainHeight,
    //                       record.NumberOfRow);
    //}
}