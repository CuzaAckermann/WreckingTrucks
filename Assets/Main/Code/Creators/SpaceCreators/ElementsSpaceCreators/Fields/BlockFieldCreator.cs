using System;
using UnityEngine;

public class BlockFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public BlockFieldCreator(LayerCreator layerCreator)
    {
        _layerCreator = layerCreator ?? throw new ArgumentNullException(nameof(layerCreator));
    }

    public event Action<Field> BlockFieldCreated;

    public Field Create(BlockSpaceSettings blockSpaceSettings)
    {
        if (blockSpaceSettings == null)
        {
            throw new ArgumentNullException(nameof(blockSpaceSettings));
        }

        Transform transform = blockSpaceSettings.FieldTransform;
        FieldSize fieldSize = blockSpaceSettings.FieldSettings.FieldSize;
        FieldIntervals fieldIntervals = blockSpaceSettings.FieldIntervals;

        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        if (fieldIntervals == null)
        {
            throw new ArgumentNullException(nameof(fieldIntervals));
        }

        Field field = new Field(_layerCreator.CreateLayers(transform, fieldSize, fieldIntervals),
                                                           transform.position,
                                                           transform.up,
                                                           transform.forward,
                                                           transform.right,
                                                           fieldIntervals.BetweenLayers,
                                                           fieldIntervals.BetweenRows,
                                                           fieldIntervals.BetweenColumns,
                                                           fieldSize.AmountColumns,
                                                           fieldSize.AmountRows);

        BlockFieldCreated?.Invoke(field);

        return field;
    }
}