using System;
using UnityEngine;

public class TruckFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public TruckFieldCreator(LayerCreator columnCreator)
    {
        _layerCreator = columnCreator ?? throw new ArgumentNullException(nameof(columnCreator));
    }

    public TruckField Create(Transform transform, FieldSize fieldSize)
    {
        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        return new TruckField(_layerCreator.CreateLayers(transform, fieldSize),
                              transform.position,
                              transform.up,
                              transform.forward,
                              transform.right,
                              fieldSize.IntervalBetweenLayers,
                              fieldSize.IntervalBetweenRows,
                              fieldSize.IntervalBetweenColumns,
                              fieldSize.AmountColumns,
                              fieldSize.AmountRows);
    }
}