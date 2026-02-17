using System;
using System.Collections.Generic;
using UnityEngine;

public class LayerCreator
{
    private readonly ColumnCreator _columnCreator;

    public LayerCreator(ColumnCreator columnCreator)
    {
        _columnCreator = columnCreator ?? throw new ArgumentNullException(nameof(columnCreator));
    }

    public List<Layer> CreateLayers(Transform transform,
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

        List<Layer> layers = new List<Layer>();

        for (int i = 0; i < fieldSize.AmountLayers; i++)
        {
            Vector3 layerPosition = transform.position + transform.up * (fieldIntervals.BetweenLayers * i);

            List<Column> columns = _columnCreator.CreateColumns(fieldSize.AmountColumns,
                                                                layerPosition,
                                                                transform.right,
                                                                fieldIntervals.BetweenColumns,
                                                                transform.forward,
                                                                fieldIntervals.BetweenRows,
                                                                fieldSize.AmountRows);

            layers.Add(new Layer(columns,
                                 layerPosition,
                                 transform.forward,
                                 fieldSize.AmountColumns,
                                 fieldSize.AmountRows));
        }

        return layers;
    }
}