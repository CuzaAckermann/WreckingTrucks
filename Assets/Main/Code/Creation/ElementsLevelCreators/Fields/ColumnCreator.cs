using System.Collections.Generic;
using UnityEngine;

public class ColumnCreator
{
    public List<Column> CreateColumns(int amountColumns,
                                      Vector3 layerPosition,
                                      Vector3 rowDirection,
                                      float intervalBetweenColumns,
                                      Vector3 columnDirection,
                                      float intervalBetweenRows,
                                      int amountRows)
    {
        List<Column> columns = new List<Column>();

        for (int column = 0; column < amountColumns; column++)
        {
            Vector3 columnPosition = layerPosition +
                                     rowDirection * (intervalBetweenColumns * column);

            columns.Add(new Column(columnPosition,
                                   columnDirection * intervalBetweenRows,
                                   amountRows));
        }

        return columns;
    }
}