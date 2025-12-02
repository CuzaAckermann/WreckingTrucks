using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FillingStrategy
{
    private readonly Stopwatch _stopwatch;

    private IFillable _field;
    private FillingCard _fillingCard;

    private bool _isSubscribed;

    public FillingStrategy(Stopwatch stopwatch, float frequency)
    {
        _stopwatch = stopwatch;
        _stopwatch.SetNotificationInterval(frequency);

        _isSubscribed = false;
    }

    public event Action FillingCardEmpty;

    public void Clear()
    {
        _fillingCard?.Clear();
    }

    public List<ColorType> GetColorType()
    {
        List<ColorType> colorTypes = new List<ColorType>();

        for (int i = 0; i < _fillingCard.Records.Count; i++)
        {
            if (colorTypes.Contains(_fillingCard.Records[i].PlaceableModel.ColorType) == false)
            {
                colorTypes.Add(_fillingCard.Records[i].PlaceableModel.ColorType);
            }
        }

        return colorTypes;
    }

    public void PrepareFilling(IFillable field, FillingCard fillingCard)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _fillingCard = fillingCard ?? throw new ArgumentNullException(nameof(fillingCard));
    }

    public void PlaceModel(Model model, int indexOfLayer, int indexOfColumn)
    {
        PlaceModel(new RecordPlaceableModel(model,
                                            indexOfLayer,
                                            indexOfColumn,
                                            _field.GetAmountModelsInColumn(indexOfLayer, indexOfColumn)));
    }

    public void Enable()
    {
        if (_isSubscribed == false)
        {
            _stopwatch.IntervalPassed += ExecuteFillingStep;
            _stopwatch.Start();

            _isSubscribed = true;
        }
    }

    public void Disable()
    {
        if (_isSubscribed)
        {
            _stopwatch.Stop();
            _stopwatch.IntervalPassed -= ExecuteFillingStep;

            _isSubscribed = false;
        }
    }

    protected abstract void Fill(FillingCard fillingCard);

    protected virtual RecordPlaceableModel GetRecord(FillingCard fillingCard)
    {
        return fillingCard.GetFirstRecord();
    }

    protected virtual Vector3 GetSpawnPosition(RecordPlaceableModel record)
    {
        return _field.Right * record.IndexOfColumn * _field.IntervalBetweenColumns +
               _field.Up * record.IndexOfLayer * _field.IntervalBetweenLayers +
               _field.Forward * _fillingCard.AmountRows * _field.IntervalBetweenRows;
    }

    protected void PlaceModel(RecordPlaceableModel record)
    {
        Vector3 spawnPosition = GetSpawnPosition(record);

        spawnPosition += _field.Position;

        record.PlaceableModel.SetFirstPosition(spawnPosition);

        _field.AddModel(record.PlaceableModel,
                        record.IndexOfLayer,
                        record.IndexOfColumn);
        _fillingCard.RemoveRecord(record);
    }

    protected void OnFillingCardEmpty()
    {
        FillingCardEmpty?.Invoke();
    }

    private void ExecuteFillingStep()
    {
        Fill(_fillingCard);
    }
}