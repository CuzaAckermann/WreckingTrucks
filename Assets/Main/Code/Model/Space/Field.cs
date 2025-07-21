using System;
using System.Collections.Generic;
using UnityEngine;

public class Field : IModelAddedNotifier,
                     IModelPositionObserver,
                     IFillable
{
    private readonly List<Layer> _layers;

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

    public void Reset()
    {
        SubscribeToLayers();
    }

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
        InsertModel(model, indexOfLayer, indexOfColumn, AmountRows - 1);
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
    }

    public void Disable()
    {
        UnsubscribeFromLayers();
    }

    public void ContinueShiftModels()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].ContinueShift();
        }
    }

    public void StopShiftModels()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].StopShift();
        }
    }

    protected virtual void OnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }

    private void SubscribeToLayers()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            SubscribeToLayer(_layers[i]);
            _layers[i].Enable();
        }
    }

    private void UnsubscribeFromLayers()
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            _layers[i].Disable();
            UnsubscribeFromLayer(_layers[i]);
        }
    }

    private void SubscribeToLayer(Layer layer)
    {
        layer.ModelAdded += OnModelAdded;
        layer.PositionChanged += OnPositionChanged;
        layer.PositionsChanged += OnPositionsChanged;
        layer.Devastated += OnDevastated;
    }

    private void UnsubscribeFromLayer(Layer layer)
    {
        layer.ModelAdded -= OnModelAdded;
        layer.PositionChanged -= OnPositionChanged;
        layer.PositionsChanged -= OnPositionsChanged;
        layer.Devastated -= OnDevastated;
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