using System;
using UnityEngine;

public class TruckFieldCreator
{
    private readonly LayerCreator _layerCreator;

    public TruckFieldCreator(LayerCreator columnCreator)
    {
        _layerCreator = columnCreator ?? throw new ArgumentNullException(nameof(columnCreator));
    }

    public TruckField Create(TruckSpaceSettings truckSpaceSettings)
    {
        if (truckSpaceSettings == null)
        {
            throw new ArgumentNullException(nameof(truckSpaceSettings));
        }

        Transform transform = truckSpaceSettings.FieldTransform;
        FieldSize fieldSize = truckSpaceSettings.FieldSettings.FieldSize;
        FieldIntervals fieldIntervals = truckSpaceSettings.FieldIntervals;

        if (fieldSize == null)
        {
            throw new ArgumentNullException(nameof(fieldSize));
        }

        if (fieldIntervals == null)
        {
            throw new ArgumentNullException(nameof(fieldIntervals));
        }

        return new TruckField(_layerCreator.CreateLayers(transform, fieldSize, fieldIntervals),
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