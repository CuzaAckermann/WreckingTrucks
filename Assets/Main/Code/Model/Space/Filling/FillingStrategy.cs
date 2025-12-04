using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class FillingStrategy
{
    private readonly Stopwatch _stopwatch;
    private readonly SpawnDetector _spawnDetector;

    private IFillable _field;
    private FillingCard _fillingCard;

    private bool _isSubscribed;

    private bool _isSubscribedToDetector;
    private bool _isWaitingDetector;

    public FillingStrategy(Stopwatch stopwatch,
                           float frequency,
                           SpawnDetector spawnDetector)
    {
        _stopwatch = stopwatch;
        _stopwatch.SetNotificationInterval(frequency);

        _spawnDetector = spawnDetector ? spawnDetector : throw new ArgumentNullException(nameof(spawnDetector));

        _isSubscribed = false;

        _isSubscribedToDetector = false;
        _isWaitingDetector = false;
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
        if (_isSubscribed == false && _isWaitingDetector == false)
        {
            _stopwatch.IntervalPassed += ExecuteFillingStep;
            _stopwatch.Start();

            _isSubscribed = true;
        }

        if (_isWaitingDetector && _isSubscribedToDetector == false)
        {
            ExecuteFillingStep();
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

        if (_isSubscribedToDetector)
        {
            _spawnDetector.Empty -= OnEmpty;
            _isSubscribedToDetector = false;
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

        if (_spawnDetector.IsEmpty(spawnPosition, -_field.Forward))
        {
            record.PlaceableModel.SetFirstPosition(spawnPosition);

            _field.AddModel(record.PlaceableModel,
                            record.IndexOfLayer,
                            record.IndexOfColumn);
            _fillingCard.RemoveRecord(record);
        }
        else
        {
            Disable();

            _spawnDetector.Empty += OnEmpty;
            _isSubscribedToDetector = true;
            _isWaitingDetector = true;
        }
    }

    protected void OnFillingCardEmpty()
    {
        FillingCardEmpty?.Invoke();
    }

    private void ExecuteFillingStep()
    {
        Fill(_fillingCard);
    }

    private void OnEmpty()
    {
        _spawnDetector.Empty -= OnEmpty;
        _isSubscribedToDetector = false;
        _isWaitingDetector = false;

        Enable();
    }
}