using System;
using System.Collections.Generic;
using UnityEngine;

public class Field : IModelSource,
                     IFillable,
                     IClearable,
                     IResetable
{
    private Vector3 _position;
    private Vector3 _columnDirection;
    private Vector3 _rowDirection;

    private float _intervalBetweenModels;
    private float _distanceBetweenModels;

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

        _position = position;
        _columnDirection = columnDirection;
        _rowDirection = rowDirection;
        _intervalBetweenModels = intervalBetweenModels;
        _distanceBetweenModels = distanceBetweenModels;

        CreateColumns(amountColumns, sizeColumn);

        SubscribeAllColumns();
    }

    public event Action<List<Model>> ModelsAdded;
    public event Action<Model> ModelAdded;
    public event Action AllColumnIsEmpty;

    public int Width => _columns.Count;

    public void PlaceModel(Model model, int numberOfColumn, int positionInColumn)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (numberOfColumn < 0 || numberOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException($"Incorrect column number {numberOfColumn}.");
        }

        _columns[numberOfColumn].AddModel(model, positionInColumn);
        ModelAdded?.Invoke(model);
    }

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

    private void CreateColumns(int amountColumns, int capacityColumn)
    {
        _columns = new List<Column>(amountColumns);

        for (int i = 0; i < amountColumns; i++)
        {
            _columns.Add(new Column(_position + _rowDirection * _intervalBetweenModels * i,
                                    _columnDirection * _distanceBetweenModels,
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
        column.AllModelsDestroyed += OnAllModelsInColumnDestroyed;
        column.TargetPositionsForModelsChanged += OnTargetPositionsForModelsChanged;
    }

    private void UnsubscribeColumn(Column column)
    {
        column.AllModelsDestroyed -= OnAllModelsInColumnDestroyed;
        column.TargetPositionsForModelsChanged -= OnTargetPositionsForModelsChanged;
    }

    private void OnAllModelsInColumnDestroyed()
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

    private void OnTargetPositionsForModelsChanged(List<Model> models)
    {
        ModelsAdded?.Invoke(models);
    }
}