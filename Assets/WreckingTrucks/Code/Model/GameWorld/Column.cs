using System;
using System.Collections.Generic;
using UnityEngine;

public class Column
{
    private readonly List<Model> _models;
    private readonly List<Model> _modelsForMovement;

    private readonly Vector3 _position;
    private readonly Vector3 _direction;
    private readonly Vector3 _directionOfModel;

    public Column(Vector3 position, Vector3 direction, int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be positive.");
        }

        _position = position;
        _direction = direction;
        _directionOfModel = -_direction;

        _models = new List<Model>(capacity);
        _modelsForMovement = new List<Model>(capacity);
    }

    public event Action<List<Model>> TargetPositionsModelsChanged;
    public event Action AllModelsDestroyed;

    public int Amount => _models.Count;

    public bool HasModels => _models.Count > 0;

    public void Clear()
    {
        for (int i = 0; i < _models.Count; i++)
        {
            if (_models[i] != null)
            {
                _models[i].Destroyed -= OnModelDestroyed;
                _models[i].Destroy();
            }
        }

        _modelsForMovement.Clear();
        _models.Clear();
    }

    public void AddModel(Model model, int positionInColumn)
    {
        _modelsForMovement.Clear();

        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Destroyed += OnModelDestroyed;
        _models.Add(model);
        model.SetDirectionForward(_directionOfModel);
        model.SetTargetPosition(CalculateModelPosition(positionInColumn));

        _modelsForMovement.Add(model);
        TargetPositionsModelsChanged?.Invoke(_modelsForMovement);
    }

    public IReadOnlyList<Model> GetModels()
    {
        return _models;
    }

    public bool TryRemoveFirstModel(Model model)
    {
        if (_models.Count > 0)
        {
            if (model == _models[0])
            {
                OnModelDestroyed(model);

                return true;
            }
        }

        return false;
    }

    public bool TryGetFirstElement(out Model model)
    {
        model = null;

        if (_models.Count > 0)
        {
            model = _models[0];

            return true;
        }

        return false;
    }

    private Vector3 CalculateModelPosition(int index)
    {
        return _position + _direction * index;
    }

    private void NullifyModel(Model model)
    {
        int index = _models.IndexOf(model);

        if (index != -1)
        {
            _models[index] = null;
        }
    }

    private void ShiftModels()
    {
        _modelsForMovement.Clear();
        Vector3 targetPosition = _position;
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

                _models[writeIndex].SetTargetPosition(targetPosition);
                _modelsForMovement.Add(_models[writeIndex]);
                targetPosition += _direction;
                writeIndex++;
            }
        }

        TrimExcessNulls(writeIndex);

        if (_modelsForMovement.Count > 0)
        {
            TargetPositionsModelsChanged?.Invoke(_modelsForMovement);
        }

        if (_models.Count == 0)
        {
            NotifyAboutEmptyModels();
        }
    }

    private void TrimExcessNulls(int startIndex)
    {
        if (startIndex < _models.Count)
        {
            _models.RemoveRange(startIndex, _models.Count - startIndex);
        }
    }

    private void OnModelDestroyed(Model model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Destroyed -= OnModelDestroyed;
        NullifyModel(model);
        ShiftModels();
    }

    private void NotifyAboutEmptyModels()
    {
        AllModelsDestroyed?.Invoke();
    }
}