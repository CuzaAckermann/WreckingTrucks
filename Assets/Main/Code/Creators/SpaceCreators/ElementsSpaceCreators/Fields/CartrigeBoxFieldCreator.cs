using System;
using UnityEngine;

public class CartrigeBoxFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public CartrigeBoxFieldCreator(LayerCreator layerCreator)
    {
        _layerCreator = layerCreator ?? throw new ArgumentNullException(nameof(layerCreator));
    }

    public CartrigeBoxField Create(CartrigeBoxSpaceSettings cartrigeBoxSpaceSettings)
    {
        if (cartrigeBoxSpaceSettings == null)
        {
            throw new ArgumentNullException(nameof(cartrigeBoxSpaceSettings));
        }

        Transform transform = cartrigeBoxSpaceSettings.FieldTransform;
        FieldSize fieldSize = cartrigeBoxSpaceSettings.FieldSettings.FieldSize;
        FieldIntervals fieldIntervals = cartrigeBoxSpaceSettings.FieldIntervals;

        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        if (fieldIntervals == null)
        {
            throw new ArgumentNullException(nameof(fieldIntervals));
        }

        return new CartrigeBoxField(_layerCreator.CreateLayers(transform, fieldSize, fieldIntervals),
                                    transform.position,
                                    transform.up,
                                    transform.forward,
                                    transform.right,
                                    fieldIntervals.BetweenLayers,
                                    fieldIntervals.BetweenRows,
                                    fieldIntervals.BetweenColumns,
                                    fieldSize.AmountColumns,
                                    fieldSize.AmountRows);
    }
}