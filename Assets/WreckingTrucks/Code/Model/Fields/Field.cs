using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field<T> : IClearable, IResetable where T : Model
{
    private Vector3 _position;
    private Vector3 _columnDirection;
    private Vector3 _rowDirection;

    private List<Column<T>> _columns;
    private Mover<T> _modelsMover;
    
    public Field(Vector3 position, Vector3 columnDirection, Vector3 rowDirection,
                 int amountColumns, int capacityColumn, int spawnPosition,
                 Mover<T> modelsMover)
    {
        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountColumns));
        }

        if (capacityColumn <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacityColumn));
        }

        _modelsMover = modelsMover ?? throw new ArgumentNullException(nameof(modelsMover));

        _position = position;
        _columnDirection = columnDirection;
        _rowDirection = rowDirection;

        CreateColumns(amountColumns, capacityColumn, spawnPosition);

        SubscribeAllColumns();
    }

    public event Action Reseted;
    public event Action<T> ModelTaken;
    public event Action CurrentAmountModelsChanged;
    public event Action TotalAmountModelsChanged;
    public event Action AllColumnIsEmpty;

    public int CurrentAmountModels {  get; private set; }

    public int TotalAmountModels { get; private set; }

    public int AmountColumns => _columns.Count;

    public void PlaceModels(List<T> models) // »«Ã≈Õ»“‹
    {
        if (models == null)
        {
            throw new ArgumentNullException(nameof(models));
        }

        for (int i = 0; i < models.Count; i++)
        {
            ModelTaken?.Invoke(models[i]);
            _columns[i].AddModel(models[i]);
        }

        CurrentAmountModels += models.Count;
        CurrentAmountModelsChanged?.Invoke();

        TotalAmountModels += models.Count;
        TotalAmountModelsChanged?.Invoke();
    }

    public void PlaceModel(T model, int numberOfColumn)
    {
        ModelTaken?.Invoke(model);
        _columns[numberOfColumn].AddModel(model);

        CurrentAmountModels++;
        CurrentAmountModelsChanged?.Invoke();

        TotalAmountModels++;
        TotalAmountModelsChanged?.Invoke();
    }

    public void Reset()
    {
        SubscribeAllColumns();

        CurrentAmountModels = 0;
        TotalAmountModels = 0;
        Reseted?.Invoke();
    }

    public void Clear()
    {
       for (int i = 0; i < _columns.Count; i++)
       {
            _columns[i].Clear();
       }

        UnsubscribeAllColumns();
    }

    public int CalculateAmountModels()
    {
        int amount = 0;

        for (int i = 0; i < _columns.Count; i++)
        {
            amount += _columns[i].AmountModels;
        }

        return amount;
    }

    protected abstract Column<T> CreateColumn(Vector3 position, Vector3 direction, int capacity, int spawnPosition);

    private void CreateColumns(int amountColumns, int capacityColumn, int spawnPosition)
    {
        _columns = new List<Column<T>>(amountColumns);

        for (int i = 0; i < amountColumns; i++)
        {
            _columns.Add(CreateColumn(_position + _rowDirection * i, _columnDirection, capacityColumn, spawnPosition));
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

    private void SubscribeColumn(Column<T> column)
    {
        column.AllModelsDestroyed += OnAllModelsInColumnDestroyed;
        column.TargetPositionsForModelsChanged += OnTargetPositionsForModelsChanged;
        column.AmountModelsChanged += OnAmountModelsChanged;
    }

    private void UnsubscribeColumn(Column<T> column)
    {
        column.AllModelsDestroyed -= OnAllModelsInColumnDestroyed;
        column.TargetPositionsForModelsChanged -= OnTargetPositionsForModelsChanged;
        column.AmountModelsChanged -= OnAmountModelsChanged;
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

    private void OnTargetPositionsForModelsChanged(List<T> models)
    {
        _modelsMover?.AddModels(models);
    }

    private void OnAmountModelsChanged()
    {
        TotalAmountModelsChanged?.Invoke();
    }
}