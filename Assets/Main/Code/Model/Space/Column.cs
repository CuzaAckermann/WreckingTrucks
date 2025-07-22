using System;
using System.Collections.Generic;
using UnityEngine;

public class Column
{
    private readonly Model[] _models;

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

        _models = new Model[capacity];
        _modelsForMovement = new List<Model>(capacity);
        _isShifting = false;
    }

    public event Action<Model> ModelAdded;
    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;
    public event Action Devastated;

    public void Clear()
    {
        for (int i = 0; i < _models.Length; i++)
        {
            if (_models[i] != null)
            {
                _models[i].Destroyed -= OnDestroyed;
                _models[i] = null;
            }
        }

        _modelsForMovement.Clear();
    }

    public void AddModel(Model model)
    {
        InsertModel(model, _models.Length - 1);
    }

    public void InsertModel(Model model, int positionInColumn)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (positionInColumn < 0 || positionInColumn >= _models.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(positionInColumn));
        }

        if (_models[positionInColumn] != null)
        {
            throw new InvalidOperationException($"{nameof(Model)} is exist in position {positionInColumn}.");
        }

        model.Destroyed += OnDestroyed;
        _models[positionInColumn] = model;
        model.SetDirectionForward(_directionOfModel);
        model.SetTargetPosition(CalculateModelPosition(positionInColumn));

        ModelAdded?.Invoke(model);
        PositionChanged?.Invoke(model);

        if (_isShifting)
        {
            ShiftModels();
        }
    }

    public bool TryGetModel(int index, out Model model)
    {
        model = null;

        if (HasModels())
        {
            if (index >= 0 && index < _models.Length)
            {
                if (_models[index] != null)
                {
                    model = _models[index];
                    return true;
                }
            }
        }

        return false;
    }

    public bool TryGetFirstModel(out Model model)
    {
        return TryGetModel(0, out model);
    }

    public bool TryGetIndexOfModel(Model model, out int index)
    {
        index = -1;

        for (int i = 0; i < _models.Length; i++)
        {
            if (_models[i] == model)
            {
                index = i;
                return true;
            }
        }

        return false;
    }

    public bool TryRemoveModel(Model removedModel)
    {
        foreach (Model model in _models)
        {
            if (model == removedModel)
            {
                OnDestroyed(model);
                return true;
            }
        }

        return false;
    }

    public int GetAmountModels()
    {
        int amount = 0;

        for (int i = 0; i < _models.Length; i++)
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

    public IReadOnlyList<Model> GetModels()
    {
        List<Model> models = new List<Model>();

        for (int i = 0; i < _models.Length; i++)
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
        _isShifting = true;

        if (HasModels())
        {
            ShiftModels();
        }
    }

    public void StopShift()
    {
        _isShifting = false;
    }

    public void OffsetPosition(float offset)
    {
        _position += _direction * offset;

        ChangePositions();
    }

    public void ReturnToBasePosition()
    {
        _position = _basePosition;

        ChangePositions();
    }

    private Vector3 CalculateModelPosition(int index)
    {
        return _position + _direction * index;
    }

    private void NullifyModel(Model nullifiedModel)
    {
        for (int i = 0; i < _models.Length; i++)
        {
            if (_models[i] == nullifiedModel)
            {
                _models[i] = null;
                return;
            }
        }
    }

    private void ShiftModels()
    {
        _modelsForMovement.Clear();
        int writeIndex = 0;

        for (int i = 0; i < _models.Length; i++)
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

        if (HasModels() == false)
        {
            OnDevastated();
        }
    }

    private void ChangePositions()
    {
        _modelsForMovement.Clear();
        int writeIndex = 0;

        for (int i = 0; i < _models.Length; i++)
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

        PositionsChanged?.Invoke(_modelsForMovement);
    }

    private void OnDestroyed(Model model)
    {
        model.Destroyed -= OnDestroyed;
        NullifyModel(model);

        if (_isShifting)
        {
            ShiftModels();
        }
    }

    private void OnDevastated()
    {
        Devastated?.Invoke();
    }
}