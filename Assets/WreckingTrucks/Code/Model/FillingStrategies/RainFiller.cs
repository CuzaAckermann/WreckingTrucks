using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RainFiller : FillingStrategy
{
    private Random _random;
    private List<int> _availableIndices = new List<int>();

    public RainFiller(IFillable field, Vector3 sourceFilling, float frequency)
               : base(field, sourceFilling, frequency)
    {
        _random = new Random();
    }

    protected override void Fill(FillingCard<Model> fillingCard)
    {
        if (fillingCard.Amount == 0)
        {
            return;
        }

        for (int i = 0; i < _random.Next(3, 5); i++)
        {
            UpdateAvailableIndices(fillingCard);

            if (_availableIndices.Count == 0)
            {
                return;
            }

            int randomListIndex = _random.Next(0, _availableIndices.Count);
            int recordIndex = _availableIndices[randomListIndex];

            RecordModelToPosition<Model> record = fillingCard.GetRecord(recordIndex);

            record.Model.SetStartPosition(new Vector3(record.LocalX, SourceFilling.y, record.LocalY));
            Field.PlaceModel(record.Model, record.LocalX, record.LocalY);
            fillingCard.RemoveRecord(record);

            _availableIndices.RemoveAt(randomListIndex);
        }
    }

    private void UpdateAvailableIndices(FillingCard<Model> fillingCard)
    {
        _availableIndices.Clear();

        for (int i = 0; i < fillingCard.Amount; i++)
        {
            _availableIndices.Add(i);
        }
    }

    public override void Clear()
    {
        base.Clear();
        _availableIndices.Clear();
    }
}