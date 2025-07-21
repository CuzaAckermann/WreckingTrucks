using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockFieldSettings : FieldSettings
{
    [SerializeField] private List<BlockLayerSettings> _layers = new List<BlockLayerSettings>();
    
    public IReadOnlyList<BlockLayerSettings> Layers => _layers;

    public void Validate()
    {
        while (_layers.Count < FieldSize.AmountLayers)
        {
            _layers.Add(new BlockLayerSettings());
        }

        while (_layers.Count > FieldSize.AmountLayers)
        {
            _layers.RemoveAt(_layers.Count - 1);
        }

        foreach (var layer in _layers)
        {
            layer?.Validate(FieldSize.AmountRows, FieldSize.AmountColumns);
        }
    }
}