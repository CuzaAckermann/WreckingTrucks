using System;
using System.Collections.Generic;
using UnityEngine;

public class Layer
{
    private readonly List<Column> _columns;
    private readonly Vector3 _direction;
    private readonly Vector3 _basePosition;

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

        _basePosition = position;
        Position = position;
        _direction = direction;

        _columns = columns ?? throw new ArgumentNullException(nameof(columns));
    }

    public event Action<int, int, Model> ModelRemoved;

    public event Action Devastated;

    public Vector3 Position { get; private set; }

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
    }

    public void InsertModel(Model model, int columnIndex, int rowIndex)
    {
        if (columnIndex < 0 || columnIndex >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(columnIndex));
        }

        _columns[columnIndex].InsertModel(model, rowIndex);
    }

    // корректировка
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

    public IReadOnlyList<Model> GetModelsInColumn(int indexOfColumn, int amountRows)
    {
        if (indexOfColumn < 0 || indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        List<Model> models = new List<Model>();

        for (int row = 0; row < amountRows; row++)
        {
            if (_columns[indexOfColumn].TryGetModel(row, out Model model))
            {
                models.Add(model);
            }
        }

        return models;
    }
    // корректировка

    public bool TryGetModel(int columnIndex, int rowIndex, out Model model)
    {
        model = null;

        if (columnIndex < 0 || columnIndex >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(columnIndex));
        }

        return _columns[columnIndex].TryGetModel(rowIndex, out model);
    }

    public bool IsEmpty(int indexOfColumn, int indexOfRow)
    {
        if (indexOfColumn < 0 || indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        return _columns[indexOfColumn].IsEmpty(indexOfRow);
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

    public int GetAmountModels()
    {
        int amountModels = 0;

        for (int column = 0; column < _columns.Count; column++)
        {
            amountModels += _columns[column].GetAmountModels();
        }

        return amountModels;
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

    public bool HasModel(Model model)
    {
        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].HasModel(model))
            {
                return true;
            }
        }

        return false;
    }

    public Model GetModel(int indexOfColumn, int indexOfRow)
    {
        if (indexOfColumn < 0 || indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        return _columns[indexOfColumn].GetModel(indexOfRow);
    }

    public void NullifyByIndex(int indexOfColumn, int indexOfRow)
    {
        if (indexOfColumn < 0 || indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        _columns[indexOfColumn].NullifyByIndex(indexOfRow);
    }

    public void ShiftColumn(int indexOfColumn)
    {
        if (indexOfColumn < 0 || indexOfColumn >= _columns.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        }

        _columns[indexOfColumn].ShiftModels();
    }

    //public void IncreaseSizeOfColumns()
    //{
    //    for (int i = 0; i < _columns.Count; i++)
    //    {
    //        _columns[i].IncreaseSize();
    //    }
    //}

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
        for (int i = 0; i < _columns.Count; i++)
        {
            _columns[i].ShiftModels();
        }
    }

    public void OffsetPosition(float offset)
    {
        Position = _direction * offset;

        for (int i = 0; i < _columns.Count; i++)
        {
            _columns[i].ShiftAmountRows(offset);
        }
    }

    public void ReturnToBasePosition()
    {
        Position = _basePosition;

        foreach (Column column in _columns)
        {
            column.ReturnToBasePosition();
        }
    }

    private int GetIndexOfColumn(Model model)
    {
        int indexOfColumn = -1;

        for (int i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].HasModel(model))
            {
                indexOfColumn = i;
                break;
            }
        }

        return indexOfColumn;
    }

    private void SubscribeToColumns()
    {
        foreach (Column column in _columns)
        {
            column.ModelRemoved += OnModelRemoved;
            column.Devastated += OnColumnDevastated;
        }
    }

    private void UnsubscribeFromSolumns()
    {
        foreach (Column column in _columns)
        {
            column.ModelRemoved -= OnModelRemoved;
            column.Devastated -= OnColumnDevastated;
        }
    }

    private void OnModelRemoved(int indexOfRow, Model model)
    {
        ModelRemoved?.Invoke(GetIndexOfColumn(model), indexOfRow, model);
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