using System;
using UnityEngine;

public class BlockFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public BlockFieldCreator(LayerCreator layerCreator)
    {
        _layerCreator = layerCreator ?? throw new ArgumentNullException(nameof(layerCreator));
    }

    public Field Create(Transform transform, FieldSize fieldSize)
    {
        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        return new Field(_layerCreator.CreateLayers(transform, fieldSize),
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