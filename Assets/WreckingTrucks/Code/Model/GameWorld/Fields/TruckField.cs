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
        GunAddedNotifier = new GunAddedNotifier();
    }

    public event Action<int> TruckRemoved;

    public GunAddedNotifier GunAddedNotifier { get; private set; }

    public override void PlaceModel(RecordModelToPosition<Model> record)
    {
        base.PlaceModel(record);

        if (record.PlaceableModel is Truck truck)
        {
            GunAddedNotifier.OnModelAdded(truck.Gun);
        }
    }

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