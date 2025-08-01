using System;
using System.Collections.Generic;
using UnityEngine;

public class Column
{
    private readonly List<Model> _models;
    private readonly List<Model> _modelsForMovement;

    private readonly Vector3 _basePosition;
    private readonly Vector3 _direction;
    private readonly Vector3 _directionOfModel;

    private Vector3 _position;

    private bool _isShifting;

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
        _modelsForMovement = new List<Model>(capacity);
        _isShifting = false;
    }

    public event Action<Model> ModelAdded;
    public event Action<int, Model> ModelRemoved;

    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;
    public event Action Devastated;

    public void Clear()
    {
        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] != null)
            {
                _models[i].Destroyed -= OnDestroyed;
                _models[i] = null;
            }
        }

        _models.Clear();
        _modelsForMovement.Clear();
    }

    public void AddModel(Model model)
    {
        if (_models[_models.Count - 1] != null)
        {
            _models.Add(null);
        }

        InsertModel(model, _models.Count - 1);
    }

    public void InsertModel(Model model, int indexOfRow)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (indexOfRow < 0 || indexOfRow >= _models.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfRow));
        }

        if (_models[indexOfRow] != null)
        {
            throw new InvalidOperationException($"{nameof(Model)} is exist in position {indexOfRow}.");
        }

        model.Destroyed += OnDestroyed;
        _models[indexOfRow] = model;
        model.SetDirectionForward(_directionOfModel);
        model.SetTargetPosition(CalculateModelPosition(indexOfRow));

        ModelAdded?.Invoke(model);
        PositionChanged?.Invoke(model);
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
        }

        return false;
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

        _models[indexOfRow].Destroyed -= OnDestroyed;
        _models[indexOfRow] = null;
    }

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
        _modelsForMovement.Clear();
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
                _modelsForMovement.Add(_models[writeIndex]);
                writeIndex++;
            }
        }

        if (_modelsForMovement.Count > 0)
        {
            PositionsChanged?.Invoke(_modelsForMovement);
        }
    }

    private Vector3 CalculateModelPosition(int index)
    {
        return _position + _direction * index;
    }

    private void OnDestroyed(Model model)
    {
        model.Destroyed -= OnDestroyed;

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