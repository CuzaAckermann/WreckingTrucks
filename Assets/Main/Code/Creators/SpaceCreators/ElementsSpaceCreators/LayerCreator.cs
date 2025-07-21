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

    public List<Layer> CreateLayers(Transform transform, FieldSize fieldSize)
    {
        List<Layer> layers = new List<Layer>();

        for (int i = 0; i < fieldSize.AmountLayers; i++)
        {
            Vector3 layerPosition = transform.position + transform.up * (fieldSize.IntervalBetweenLayers * i);

            List<Column> columns = _columnCreator.CreateColumns(fieldSize.AmountColumns,
                                                                layerPosition,
                                                                transform.right,
                                                                fieldSize.IntervalBetweenColumns,
                                                                transform.forward,
                                                                fieldSize.IntervalBetweenRows,
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