using System;
using UnityEngine;

public abstract class FillingStrategy
{
    private IFillable _field;
    private FillingCard _fillingCard;
    
    public FillingStrategy(float frequency)
    {
        if (frequency <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(frequency));
        }

        Frequency = frequency;
    }

    public event Action FillingCardEmpty;

    public float Frequency { get; private set; }

    public void Clear()
    {
        _fillingCard?.Clear();
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

    public void ExecuteFillingStep()
    {
        Fill(_fillingCard);
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

        record.PlaceableModel.SetPosition(spawnPosition);
        _field.AddModel(record.PlaceableModel,
                        record.IndexOfLayer,
                        record.IndexOfColumn);
        _fillingCard.RemoveRecord(record);
    }

    protected void OnFillingCardEmpty()
    {
        FillingCardEmpty?.Invoke();
    }
}