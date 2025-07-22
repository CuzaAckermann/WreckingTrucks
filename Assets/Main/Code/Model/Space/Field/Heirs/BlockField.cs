using System.Collections.Generic;
using UnityEngine;

public class BlockField : Field
{
    public BlockField(List<Layer> layers,
                      Vector3 position,
                      Vector3 layerDirection,
                      Vector3 columnDirection,
                      Vector3 rowDirection, 
                      float intervalBetweenLayers,
                      float intervalBetweenRows,
                      float intervalBetweenColumns,
                      int amountColumns,
                      int sizeColumn)
               : base(layers,
                      position,
                      layerDirection,
                      columnDirection,
                      rowDirection,
                      intervalBetweenLayers,
                      intervalBetweenRows,
                      intervalBetweenColumns,
                      amountColumns,
                      sizeColumn)
    {

    }
}