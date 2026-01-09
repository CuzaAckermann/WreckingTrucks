using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Column
{
    private readonly List<Model> _models;

    private readonly Vector3 _basePosition;
    private readonly Vector3 _direction;
    private readonly Vector3 _directionOfModel;

    private Vector3 _position;

    public Column(Vector3 position, Vector3 direction, int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be positive.");
        }

        _basePosition = position;
        _position = position;
        _direction = direction;
        _directionOfModel = -_direction;

        _models = new List<Model>(capacity) { null };
    }

    public event Action<int, Model> ModelRemoved;

    public event Action Devastated;

    public void Clear()
    {
        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] != null)
            {
                _models[i].DestroyedModel -= OnDestroyed;
                _models[i] = null;
            }
        }

        _models.Clear();
    }

    public void AddModel(Model model)
    {
        if (_models[_models.Count - 1] != null)
        {
            _models.Add(null);
        }

        int indexOfPosition = GetAmountModels();

        while (_models[indexOfPosition] != null)
        {
            indexOfPosition++;
            Logger.Log(indexOfPosition);
        }

        InsertModel(model, indexOfPosition);
    }

    public void InsertModel(Model model, int indexOfRow)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (indexOfRow < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfRow));
        }

        if (indexOfRow >= _models.Count)
        {
            //Logger.Log($"{nameof(indexOfRow)} is not less than amount of {nameof(_models)}");

            int amountNulls = indexOfRow - (_models.Count - 1);

            for (int i = 0; i < amountNulls; i++)
            {
                _models.Add(null);
            }
        }

        if (_models[indexOfRow] != null)
        {
            throw new InvalidOperationException($"{nameof(Model)} is exist in position {indexOfRow}.");
        }

        model.DestroyedModel += OnDestroyed;
        _models[indexOfRow] = model;
        model.SetDirectionForward(_directionOfModel);
        model.SetTargetPosition(CalculateModelPosition(indexOfRow));
    }

    public bool TryGetModel(int index, out Model model)
    {
        model = null;

        if (index >= 0 && index < _models.Count)
        {
            if (_models[index] != null)
            {
                model = _models[index];
                return true;
            }
            else
            {
                //Logger.Log(index);
            }
        }

        return false;
    }

    public bool IsEmpty(int indexOfRow)
    {
        if (indexOfRow >= 0 && indexOfRow < _models.Count)
        {
            if (_models[indexOfRow] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return true;
    }

    public bool TryGetFirstModel(out Model model)
    {
        return TryGetModel(0, out model);
    }

    public bool TryGetIndexOfModel(Model model, out int indexOfRow)
    {
        indexOfRow = -1;

        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] == model)
            {
                indexOfRow = i;
                return true;
            }
        }

        return false;
    }

    public bool TryRemoveModel(Model removedModel)
    {
        for (int i = _models.Count - 1; i >= 0; i--)
        {
            if (_models[i] == removedModel)
            {
                OnDestroyed(_models[i]);

                return true;
            }
        }

        return false;
    }

    public int GetAmountModels()
    {
        int amount = 0;

        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] != null)
            {
                amount++;
            }
        }

        return amount;
    }

    public bool HasModels()
    {
        foreach (Model model in _models)
        {
            if (model != null)
            {
                return true;
            }
        }

        return false;
    }

    public bool HasModel(Model model)
    {
        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] == model)
            {
                return true;
            }
        }

        return false;
    }

    public Model GetModel(int indexOfRow)
    {
        if (indexOfRow < 0 || indexOfRow >= _models.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfRow));
        }

        return _models[indexOfRow];
    }

    public IReadOnlyList<Model> GetModels()
    {
        List<Model> models = new List<Model>();

        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] != null)
            {
                models.Add(_models[i]);
            }
        }

        return models;
    }

    public void ContinueShift()
    {
        ShiftModels();
    }

    public void ShiftAmountRows(float rows)
    {
        _position += _direction * rows;

        ChangePositions();
    }

    public void ReturnToBasePosition()
    {
        _position = _basePosition;

        ChangePositions();
    }

    public void NullifyByIndex(int indexOfRow)
    {
        if (indexOfRow < 0 || indexOfRow >= _models.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfRow));
        }

        _models[indexOfRow].DestroyedModel -= OnDestroyed;
        _models[indexOfRow] = null;
    }

    //public void IncreaseSize()
    //{
    //    _models.Add(null);
    //}

    public void ShiftModels()
    {
        ChangePositions();

        if (HasModels() == false)
        {
            OnDevastated();
        }
    }

    private void ChangePositions()
    {
        int writeIndex = 0;

        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] != null)
            {
                if (writeIndex != i)
                {
                    _models[writeIndex] = _models[i];
                    _models[i] = null;
                }

                _models[writeIndex].SetTargetPosition(CalculateModelPosition(writeIndex));
                writeIndex++;
            }
        }
    }

    private Vector3 CalculateModelPosition(int index)
    {
        return _position + _direction * index;
    }

    private void OnDestroyed(Model model)
    {
        model.DestroyedModel -= OnDestroyed;

        if (TryGetIndexOfModel(model, out int rowIndex) == false)
        {
            return;
        }

        ModelRemoved?.Invoke(rowIndex, model);
    }

    private void OnDevastated()
    {
        Devastated?.Invoke();
    }
}