using System;
using UnityEngine;

public class CartrigeBoxFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public CartrigeBoxFieldCreator(LayerCreator layerCreator)
    {
        _layerCreator = layerCreator ?? throw new ArgumentNullException(nameof(layerCreator));
    }

    public CartrigeBoxField Create(Transform transform, FieldSize fieldSize)
    {
        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        return new CartrigeBoxField(_layerCreator.CreateLayers(transform, fieldSize),
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