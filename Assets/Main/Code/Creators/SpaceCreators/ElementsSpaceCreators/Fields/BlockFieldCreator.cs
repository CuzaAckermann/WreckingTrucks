using System;
using UnityEngine;

public class BlockFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public BlockFieldCreator(LayerCreator layerCreator)
    {
        _layerCreator = layerCreator ?? throw new ArgumentNullException(nameof(layerCreator));
    }

    public BlockField Create(Transform transform,
                             FieldSize fieldSize,
                             FieldIntervals fieldIntervals,
                             EventBus eventBus)
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

        BlockField field = new BlockField(_layerCreator.CreateLayers(transform, fieldSize, fieldIntervals),
                                                                     transform.position,
                                                                     transform.up,
                                                                     transform.forward,
                                                                     transform.right,
                                                                     fieldIntervals.BetweenLayers,
                                                                     fieldIntervals.BetweenRows,
                                                                     fieldIntervals.BetweenColumns,
                                                                     fieldSize.AmountColumns,
                                                                     fieldSize.AmountRows,
                                                                     eventBus);

        eventBus.Invoke(new CreatedBlockFieldSignal(field));

        return field;
    }
}