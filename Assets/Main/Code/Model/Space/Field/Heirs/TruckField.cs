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
                      int sizeColumn,
                      EventBus eventBus)
               : base(layers,
                      position,
                      layerDirection,
                      columnDirection,
                      rowDirection,
                      intervalBetweenLayers,
                      intervalBetweenRows,
                      intervalBetweenColumns, 
                      amountColumns,
                      sizeColumn,
                      eventBus)
    {
        ContinueShiftModels();
    }

    public event Action<int, int> TruckRemoved;

    public bool IsFirstInRow(Model model)
    {
        if (TryGetIndexModel(model, out int _, out int _, out int indexOfRow))
        {
            if (indexOfRow == 0)
            {
                return true;
            }
        }

        return false;
    }

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

    protected override DevastatedFieldSignal InvokeDevastated()
    {
        return new DevastatedTruckFieldSignal();
    }
}