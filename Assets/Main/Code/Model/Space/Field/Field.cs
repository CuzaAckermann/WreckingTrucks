using System;
using System.Collections.Generic;
using UnityEngine;

public class Field : IModelAddedNotifier,
                     IModelPositionObserver,
                     IFillable
{
    private readonly List<Layer> _layers;
    private readonly List<Model> _modelsForMovement;

    private bool _isShifting;

    public Field(List<Layer> layers,
                 Vector3 position,
                 Vector3 layerDirection,
                 Vector3 columnDirection,
                 Vector3 rowDirection,
                 float intervalBetweenLayers,
                 float intervalBetweenRows,
                 float intervalBetweenColumns,
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

        if (intervalBetweenLayers <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalBetweenLayers));
        }

        if (intervalBetweenColumns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalBetweenColumns));
        }

        if (intervalBetweenRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalBetweenRows));
        }

        Position = position;

        IntervalBetweenLayers = intervalBetweenLayers;
        IntervalBetweenColumns = intervalBetweenColumns;
        IntervalBetweenRows = intervalBetweenRows;

        Forward = columnDirection;
        Right = rowDirection;
        Up = layerDirection;

        AmountColumns = amountColumns;
        AmountRows = sizeColumn;

        _layers = layers ?? throw new ArgumentNullException(nameof(layers));
        _modelsForMovement = new List<Model>(_layers.Count);
        _isShifting = false;
    }

    public event Action<Model> ModelAdded;
    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;
    public event Action Devastated;

    public Vector3 Position { get; private set; }

    public Vector3 Forward { get; private set; }

    public Vector3 Right { get; private set; }

    public Vector3 Up { get; private set; }

    public float IntervalBetweenLayers { get; private set; }

    public float IntervalBetweenColumns { get; private set; }

    public float IntervalBetweenRows { get; private set; }

    public int AmountLayers => _layers.Count;

    public int AmountColumns { get; private set; }

    public int AmountRows { get; private set; }

    protected IReadOnlyList<Layer> Layers => _layers;

    public void Clear()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].Clear();
        }

        UnsubscribeFromLayers();
    }

    public void AddModel(Model model, int indexOfLayer, int indexOfColumn)
    {
        if (indexOfLayer < 0 || indexOfLayer >= _layers.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLayer));
        }

        _layers[indexOfLayer].AddModel(model, indexOfColumn);
    }

    public void InsertModel(Model model,
                            int indexOfLayer,
                            int indexOfColumn,
                            int indexOfRow)
    {
        if (indexOfLayer < 0 || indexOfLayer >= _layers.Count)
        {
            throw new ArgumentOutOfRangeException($"Incorrect layer number {indexOfLayer}.");
        }

        _layers[indexOfLayer].InsertModel(model, indexOfColumn, indexOfRow);
    }

    public IReadOnlyList<Model> GetModels(int amountRows)
    {
        List<Model> models = new List<Model>();

        for (int layer = 0; layer < _layers.Count; layer++)
        {
            models.AddRange(_layers[layer].GetModels(amountRows));
        }

        return models;
    }

    public bool TryGetFirstModel(int indexOfLayer, int indexOfColumn, out Model model)
    {
        if (indexOfLayer < 0 || indexOfLayer >= _layers.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLayer));
        }

        return _layers[indexOfLayer].TryGetFirstModel(indexOfColumn, out model);
    }

    public List<Model> GetFirstModels()
    {
        List<Model> models = new List<Model>();

        for (int i = 0; i < _layers.Count; i++)
        {
            models.AddRange(_layers[i].GetFirstModels());
        }

        return models;
    }

    public bool TryGetIndexModel(Model model,
                                 out int indexOfLayer,
                                 out int indexOfColumn,
                                 out int indexOfRow)
    {
        indexOfLayer = -1;
        indexOfColumn = -1;
        indexOfRow = -1;

        for (int i = 0; i < _layers.Count; i++)
        {
            if (_layers[i].TryGetIndexModel(model, out indexOfColumn, out indexOfRow))
            {
                indexOfLayer = i;

                return true;
            }
        }

        return false;
    }

    public bool TryRemoveModel(Model model)
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            if (_layers[i].TryRemoveModel(model))
            {
                return true;
            }
        }

        return false;
    }

    public int GetAmountModelsInColumn(int indexOfLayer, int indexOfColumn)
    {
        if (indexOfLayer < 0 || indexOfLayer >= _layers.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLayer));
        }

        return _layers[indexOfLayer].GetAmountModels(indexOfColumn);
    }

    public IReadOnlyList<Model> GetModels()
    {
        List<Model> models = new List<Model>();

        for (int i = 0; i < _layers.Count; i++)
        {
            models.AddRange(_layers[i].GetModels());
        }

        return models;
    }

    public void Enable()
    {
        SubscribeToLayers();

        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].Enable();
        }
    }

    public void Disable()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].Disable();
        }

        UnsubscribeFromLayers();
    }

    public void ContinueShiftModels()
    {
        _isShifting = true;

        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].ContinueShift();
        }
    }

    public void StopShiftModels()
    {
        _isShifting = false;
    }

    public void ShowRows(int amountRows)
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            float layerOffset = i * amountRows * IntervalBetweenRows;
            _layers[i].OffsetPosition(layerOffset);
        }
    }

    public void HideRows()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].ReturnToBasePosition();
        }
    }

    protected virtual void OnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }

    private int GetIndexOfLayer(Model model)
    {
        int indexOfLayer = -1;

        for (int i = 0; i < _layers.Count; i++)
        {
            if (_layers[i].HasModel(model))
            {
                indexOfLayer = i;
                break;
            }
        }

        return indexOfLayer;
    }

    private void SubscribeToLayers()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            SubscribeToLayer(_layers[i]);
        }
    }

    private void UnsubscribeFromLayers()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            UnsubscribeFromLayer(_layers[i]);
        }
    }

    private void SubscribeToLayer(Layer layer)
    {
        layer.ModelAdded += OnModelAdded;
        layer.ModelRemoved += OnModelRemoved;

        layer.PositionChanged += OnPositionChanged;
        layer.PositionsChanged += OnPositionsChanged;
        layer.Devastated += OnDevastated;
    }

    private void UnsubscribeFromLayer(Layer layer)
    {
        layer.ModelAdded -= OnModelAdded;
        layer.ModelRemoved -= OnModelRemoved;

        layer.PositionChanged -= OnPositionChanged;
        layer.PositionsChanged -= OnPositionsChanged;
        layer.Devastated -= OnDevastated;
    }

    private void OnModelRemoved(int indexOfColumn, int indexOfRow, Model model)
    {
        int indexOfLayer = GetIndexOfLayer(model);
        _layers[indexOfLayer].NullifyByIndex(indexOfColumn, indexOfRow);

        if (_isShifting)
        {
            ShiftLayers(indexOfLayer, indexOfColumn, indexOfRow);

            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].ShiftColumn(indexOfColumn);
            }
        }
    }

    private void ShiftLayers(int indexOfLayer, int indexOfColumn, int indexOfRow)
    {
        _modelsForMovement.Clear();
        int writeIndex = indexOfLayer;

        for (int i = indexOfLayer; i < _layers.Count; i++)
        {
            if (_layers[i].TryGetModel(indexOfColumn, indexOfRow, out Model model))
            {
                if (writeIndex != i)
                {
                    _layers[writeIndex].InsertModel(model, indexOfColumn, indexOfRow);
                    _layers[i].NullifyByIndex(indexOfColumn, indexOfRow);
                }

                _modelsForMovement.Add(model);
                writeIndex++;
            }
        }

        if (_modelsForMovement.Count > 0)
        {
            PositionsChanged?.Invoke(_modelsForMovement);
        }
    }

    private void OnPositionChanged(Model model)
    {
        PositionChanged?.Invoke(model);
    }

    private void OnPositionsChanged(List<Model> models)
    {
        PositionsChanged?.Invoke(models);
    }

    private void OnDevastated()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            if (_layers[i] != null)
            {
                if (_layers[i].HasModels())
                {
                    return;
                }
            }
        }

        Devastated?.Invoke();
    }
}