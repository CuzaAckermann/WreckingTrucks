using System;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    private readonly List<Column> _columns;
    private readonly Vector3 _position;
    private readonly Vector3 _direction;

    public Layer(List<Column> columns,
                 Vector3 position,
                 Vector3 direction,
                 int amountColumns,
                 int sizeColumn)
    {
        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountColumns));
        }

        if (sizeColumn <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sizeColumn));
        }

        _position = position;
        _direction = direction;

        _columns = columns ?? throw new ArgumentNullException(nameof(columns));
    }

    public event Action<Model> ModelAdded;
    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;
    public event Action Devastated;

    public Vector3 Position => _position;

    public Vector3 Direction => _direction;

    public int AmountColumns => _columns.Count;

    public IReadOnlyList<Column> Columns => _columns;

    public void Clear()
    {
        foreach (var column in _columns)
        {
            column.Clear();
        }

        UnsubscribeFromSolumns();
    }

    public void AddModel(Model model, int columnIndex)
    {
        if (columnIndex < 0 || columnIndex >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(columnIndex));
        }

        _columns[columnIndex].AddModel(model);
        ModelAdded?.Invoke(model);
    }

    public void InsertModel(Model model, int columnIndex, int rowIndex)
    {
        if (columnIndex < 0 || columnIndex >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(columnIndex));
        }

        _columns[columnIndex].InsertModel(model, rowIndex);
        ModelAdded?.Invoke(model);
    }

    public IReadOnlyList<Model> GetModels(int amountRows)
    {
        List<Model> models = new List<Model>();

        for (int row = 0; row < amountRows; row++)
        {
            for (int column = 0; column < _columns.Count; column++)
            {
                if (_columns[column].TryGetModel(row, out Model model))
                {
                    models.Add(model);
                }
            }
        }

        return models;
    }

    public bool TryGetModel(int columnIndex, int rowIndex, out Model model)
    {
        model = null;

        if (columnIndex < 0 || columnIndex >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(columnIndex));
        }

        return _columns[columnIndex].TryGetModel(rowIndex, out model);
    }

    public bool TryGetFirstModel(int indexOfColumn, out Model model)
    {
        if (indexOfColumn < 0 || indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        return _columns[indexOfColumn].TryGetFirstModel(out model);
    }

    public List<Model> GetFirstModels()
    {
        List<Model> models = new List<Model>();

        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].TryGetFirstModel(out Model model))
            {
                models.Add(model);
            }
        }

        return models;
    }

    public bool TryGetIndexModel(Model model, out int indexOfColumn, out int indexOfRow)
    {
        indexOfColumn = -1;
        indexOfRow = -1;

        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].TryGetIndexOfModel(model, out indexOfRow))
            {
                indexOfColumn = i;

                return true;
            }
        }

        return false;
    }

    public bool TryRemoveModel(Model model)
    {
        foreach (Column column in _columns)
        {
            if (column.TryRemoveModel(model))
            {
                return true;
            }
        }

        return false;
    }

    public int GetAmountModels(int indexOfColumn)
    {
        if (indexOfColumn < 0 || indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        return _columns[indexOfColumn].GetAmountModels();
    }

    public bool HasModels()
    {
        foreach (Column column in _columns)
        {
            if (column.HasModels())
            {
                return true;
            }
        }

        return false;
    }

    public IReadOnlyList<Model> GetModels()
    {
        List<Model> models = new List<Model>();

        for (int i = 0; i < _columns.Count; i++)
        {
            models.AddRange(_columns[i].GetModels());
        }

        return models;
    }

    public void Enable()
    {
        SubscribeToColumns();
    }

    public void Disable()
    {
        UnsubscribeFromSolumns();
    }

    public void ContinueShift()
    {
        foreach (Column column in _columns)
        {
            column.ContinueShift();
        }
    }

    public void StopShift()
    {
        foreach (Column column in _columns)
        {
            column.StopShift();
        }
    }

    private void SubscribeToColumns()
    {
        foreach (Column column in _columns)
        {
            column.ModelAdded += OnColumnModelAdded;
            column.PositionChanged += OnColumnPositionChanged;
            column.PositionsChanged += OnColumnPositionsChanged;
            column.Devastated += OnColumnDevastated;
        }
    }

    private void UnsubscribeFromSolumns()
    {
        foreach (Column column in _columns)
        {
            column.ModelAdded -= OnColumnModelAdded;
            column.PositionChanged -= OnColumnPositionChanged;
            column.PositionsChanged -= OnColumnPositionsChanged;
            column.Devastated -= OnColumnDevastated;
        }
    }

    private void OnColumnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }

    private void OnColumnPositionChanged(Model model)
    {
        PositionChanged?.Invoke(model);
    }

    private void OnColumnPositionsChanged(List<Model> models)
    {
        PositionsChanged?.Invoke(models);
    }

    private void OnColumnDevastated()
    {
        foreach (Column column in _columns)
        {
            if (column.HasModels())
            {
                return;
            }
        }

        Devastated?.Invoke();
    }
}