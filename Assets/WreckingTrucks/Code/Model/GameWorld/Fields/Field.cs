using System;
using System.Collections.Generic;
using UnityEngine;

public class Field : IModelAddedNotifier,
                     IPositionsModelsChangedNotifier,
                     IFillable,
                     IClearable,
                     IResetable
{
    private Vector3 _columnDirection;
    private Vector3 _rowDirection;

    private List<Column> _columns;
    
    public Field(Vector3 position, Vector3 columnDirection, Vector3 rowDirection,
                 float intervalBetweenModels, float distanceBetweenModels,
                 int amountColumns, int sizeColumn)
    {
        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountColumns));
        }

        if (sizeColumn <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeColumn));
        }

        if (intervalBetweenModels <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalBetweenModels));
        }

        if (distanceBetweenModels <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(distanceBetweenModels));
        }

        Position = position;
        _columnDirection = columnDirection;
        _rowDirection = rowDirection;
        IntervalBetweenModels = intervalBetweenModels;
        DistanceBetweenModels = distanceBetweenModels;

        Forward = columnDirection;
        Right = rowDirection;
        Up = Vector3.up;

        CreateColumns(amountColumns, sizeColumn);

        SubscribeAllColumns();
    }

    public event Action<List<Model>> TargetPositionsModelsChanged;
    public event Action<Model> ModelAdded;
    public event Action AllColumnIsEmpty;

    public Vector3 Position { get; private set; }

    public Vector3 Forward {  get; private set; }

    public Vector3 Right { get; private set; }

    public Vector3 Up { get; private set; }

    public float IntervalBetweenModels { get; private set; }

    public float DistanceBetweenModels { get; private set; }

    public int AmountColumn => _columns.Count;

    protected IReadOnlyList<Column> Columns => _columns;

    public void Reset()
    {
        SubscribeAllColumns();
    }

    public void Clear()
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            _columns[i].Clear();
        }

        UnsubscribeAllColumns();
    }

    public void PlaceModel(RecordModelToPosition<Model> record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        Model placableModel = record.PlaceableModel;
        int numberOfColumns = record.NumberOfColumn;
        int numberOfRows = record.NumberOfRow;

        if (placableModel == null)
        {
            throw new ArgumentNullException(nameof(placableModel));
        }

        if (numberOfColumns < 0 || numberOfColumns >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException($"Incorrect column number {numberOfColumns}.");
        }

        _columns[numberOfColumns].AddModel(placableModel, numberOfRows);
        ModelAdded?.Invoke(placableModel);
    }

    public void PlaceModel(Model model, int numberOfColumn)
    {
        PlaceModel(new RecordModelToPosition<Model>(model, _columns[numberOfColumn].Amount, numberOfColumn));
    }

    public IReadOnlyList<Model> GetModels()
    {
        List<Model> blocks = new List<Model>();

        foreach (Column column in _columns)
        {
            blocks.AddRange(column.GetModels());
        }

        return blocks;
    }

    public int GetAmountElementsInColumn(int columnIndex)
    {
        return _columns[columnIndex].Amount;
    }

    public bool TryGetFirstElement(int indexOfColumn, out Model model)
    {
        if (indexOfColumn < 0 && indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        return _columns[indexOfColumn].TryGetFirstElement(out model);
    }

    private void CreateColumns(int amountColumns, int capacityColumn)
    {
        _columns = new List<Column>(amountColumns);

        for (int i = 0; i < amountColumns; i++)
        {
            _columns.Add(new Column(Position + _rowDirection * IntervalBetweenModels * i,
                                    _columnDirection * DistanceBetweenModels,
                                    capacityColumn));
        }
    }

    private void SubscribeAllColumns()
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            SubscribeColumn(_columns[i]);
        }
    }

    private void UnsubscribeAllColumns()
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            UnsubscribeColumn(_columns[i]);
        }
    }

    private void SubscribeColumn(Column column)
    {
        column.AllModelsDestroyed += OnAllModelsDestroyed;
        column.TargetPositionsModelsChanged += OnTargetPositionsModelsChanged;
    }

    private void UnsubscribeColumn(Column column)
    {
        column.AllModelsDestroyed -= OnAllModelsDestroyed;
        column.TargetPositionsModelsChanged -= OnTargetPositionsModelsChanged;
    }

    private void OnAllModelsDestroyed()
    {
        int amountColumnsWithModels = 0;

        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i] != null)
            {
                if (_columns[i].HasModels)
                {
                    amountColumnsWithModels++;
                }
            }
        }

        if (amountColumnsWithModels == 0)
        {
            AllColumnIsEmpty?.Invoke();
        }
    }

    private void OnTargetPositionsModelsChanged(List<Model> models)
    {
        TargetPositionsModelsChanged?.Invoke(models);
    }
}