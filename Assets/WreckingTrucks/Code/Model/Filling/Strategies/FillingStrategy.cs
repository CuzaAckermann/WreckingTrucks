using System;
using UnityEngine;

public abstract class FillingStrategy
{
    private IFillable _field;
    private FillingCard<Model> _fillingCard;
    private float _timeLastFill;
    private float _frequency;
    private bool _isFilling;

    public FillingStrategy(float frequency)
    {
        if (frequency <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(frequency));
        }

        _frequency = frequency;
        _isFilling = false;
    }

    public void Clear()
    {
        _fillingCard?.Clear();
    }

    public void StartFilling(IFillable field, FillingCard<Model> fillingCard)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _fillingCard = fillingCard ?? throw new ArgumentNullException(nameof(fillingCard));
        _timeLastFill = 0;
        _isFilling = true;
    }

    public void PlaceModel(Model model, int numberOfColumn)
    {
        PlaceModel(new RecordModelToPosition<Model>(model,
                                                    _field.GetAmountElementsInColumn(numberOfColumn),
                                                    numberOfColumn));
    }

    public void Tick(float deltaTime)
    {
        if (_isFilling == false)
        {
            return;
        }

        _timeLastFill += deltaTime;

        if (_timeLastFill >= _frequency)
        {
            Fill(_fillingCard);
            _timeLastFill -= _frequency;

            if (_fillingCard.Amount == 0)
            {
                _isFilling = false;
            }
        }
    }

    protected abstract void Fill(FillingCard<Model> fillingCard);

    protected virtual RecordModelToPosition<Model> GetRecordModelToPosition(FillingCard<Model> fillingCard)
    {
        return fillingCard.GetFirstRecord();
    }

    protected virtual Vector3 GetSpawnPosition(RecordModelToPosition<Model> record)
    {
        return _field.Right * record.NumberOfColumn * _field.IntervalBetweenModels +
               _field.Up * 0 +
               _field.Forward * _fillingCard.Length * _field.DistanceBetweenModels;
    }

    protected void PlaceModel(RecordModelToPosition<Model> record)
    {
        Vector3 spawnPosition = GetSpawnPosition(record);

        spawnPosition += _field.Position;

        record.PlaceableModel.SetStartPosition(spawnPosition);
        _field.PlaceModel(record);
        _fillingCard.RemoveRecord(record);
    }
}