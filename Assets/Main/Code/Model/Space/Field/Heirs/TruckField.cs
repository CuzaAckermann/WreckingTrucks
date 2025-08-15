using System;
using System.Collections.Generic;
using UnityEngine;

public class TruckField : Field
{
    public TruckField(List<Layer> layers,
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

    public event Action<int, int> TruckRemoved;

    public bool TryRemoveTruck(Truck truck)
    {
        for (int i = 0; i < Layers.Count; i++)
        {
            if (TryGetIndexModel(truck, out int indexOfLayer,
                                        out int indexOfColumn,
                                        out int _))
            {
                if (Layers[i].TryRemoveModel(truck))
                {
                    TruckRemoved?.Invoke(indexOfLayer, indexOfColumn);
                    return true;
                }
            }
        }

        return false;
    }
}