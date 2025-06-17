using System;
using UnityEngine;

public class TruckField : Field
{
    public TruckField(Vector3 position, Vector3 columnDirection, Vector3 rowDirection,
                      float intervalBetweenModels, float distanceBetweenModels,
                      int amountColumns, int sizeColumn)
               : base(position, columnDirection, rowDirection,
                      intervalBetweenModels, distanceBetweenModels,
                      amountColumns, sizeColumn)
    {

    }

    public event Action<int> TruckRemoved;

    public bool TryRemoveTruck(Model model)
    {
        for (int i = 0; i < Columns.Count; i++)
        {
            if (Columns[i].TryRemoveFirstModel(model))
            {
                TruckRemoved?.Invoke(i);
                return true;
            }
        }

        return false;
    }
}