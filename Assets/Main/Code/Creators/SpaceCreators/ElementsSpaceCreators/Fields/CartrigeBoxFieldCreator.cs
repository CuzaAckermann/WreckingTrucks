using System;
using UnityEngine;

public class CartrigeBoxFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public CartrigeBoxFieldCreator(LayerCreator layerCreator)
    {
        _layerCreator = layerCreator ?? throw new ArgumentNullException(nameof(layerCreator));
    }

    public event Action<CartrigeBoxField> Created;

    public CartrigeBoxField Create(Transform transform,
                                   FieldSize fieldSize,
                                   FieldIntervals fieldIntervals)
    {
        if (transform == null)
        {
            throw new ArgumentNullException(nameof(transform));
        }

        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        if (fieldIntervals == null)
        {
            throw new ArgumentNullException(nameof(fieldIntervals));
        }

        CartrigeBoxField cartrigeBoxField = new CartrigeBoxField(_layerCreator.CreateLayers(transform, fieldSize, fieldIntervals),
                                                                 transform.position,
                                                                 transform.up,
                                                                 transform.forward,
                                                                 transform.right,
                                                                 fieldIntervals.BetweenLayers,
                                                                 fieldIntervals.BetweenRows,
                                                                 fieldIntervals.BetweenColumns,
                                                                 fieldSize.AmountColumns,
                                                                 fieldSize.AmountRows);

        Created?.Invoke(cartrigeBoxField);

        return cartrigeBoxField;
    }
}