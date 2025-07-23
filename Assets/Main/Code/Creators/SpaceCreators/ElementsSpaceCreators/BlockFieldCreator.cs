using System;
using UnityEngine;

public class BlockFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public BlockFieldCreator(LayerCreator layerCreator)
    {
        _layerCreator = layerCreator ?? throw new ArgumentNullException(nameof(layerCreator));
    }

    public Field Create(Transform transform,
                        FieldSize fieldSize,
                        FieldIntervals fieldIntervals)
    {
        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        if (fieldIntervals == null)
        {
            throw new ArgumentNullException(nameof(fieldIntervals));
        }

        return new Field(_layerCreator.CreateLayers(transform, fieldSize, fieldIntervals),
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