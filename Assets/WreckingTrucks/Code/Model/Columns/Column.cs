using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Column<T> where T : Model
{
    private readonly List<T> _models;
    private readonly List<T> _modelsForMovement;

    private readonly Vector3 _position;
    private readonly Vector3 _direction;
    private readonly int _spawnPosition;

    public Column(Vector3 position, Vector3 direction, int capacity, int spawnPosition)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be positive.");
        }

        _position = position;
        _direction = direction;

        _models = new List<T>(capacity);
        _modelsForMovement = new List<T>(capacity);
        _spawnPosition = spawnPosition;
    }

    public event Action AmountModelsChanged;
    public event Action<List<T>> TargetPositionsForModelsChanged;
    public event Action AllModelsDestroyed;

    public bool HasModels => _models.Count > 0;

    public int AmountModels => _models.Count;

    public void AddModel(T model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        model.Destroyed += OnModelDestroyed;
        _models.Add(model);

        // позици€ находитьс€ слишком далеко, что делать если нужно будет использовать разные позиции дл€ спавна блоков, например дл€ ƒќ∆ƒя, »«ћ≈Ќ»“№
        model.SetStartPosition(CalculateModelPosition(Mathf.Min(_spawnPosition, _models.Capacity))); 

        model.SetTargetPosition(CalculateModelPosition(_models.Count - 1));
        ShiftModels();
    }

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

    private Vector3 CalculateModelPosition(int index)
    {
        return _position + _direction * index;
    }

    private void NullifyModel(T model)
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

        AmountModelsChanged?.Invoke();

        if (_modelsForMovement.Count > 0)
        {
            TargetPositionsForModelsChanged?.Invoke(_modelsForMovement);
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
        NullifyModel((T)model);
        ShiftModels();
    }

    private void NotifyAboutEmptyModels()
    {
        AllModelsDestroyed?.Invoke();
    }
}