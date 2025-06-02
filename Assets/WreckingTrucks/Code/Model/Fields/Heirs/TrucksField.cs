using System;
using UnityEngine;

public class TrucksField : Field<Truck>
{
    public TrucksField(Vector3 position, Vector3 columnDirection, Vector3 rowDirection,
                       int amountColumns, int capacityColumn, int spawnPosition,
                       Mover<Truck> trucksMover)
                : base(position, columnDirection, rowDirection,
                       amountColumns, capacityColumn, spawnPosition,
                       trucksMover)
    {

    }

    //public event Action PlaceHasBeenVacated;

    protected override Column<Truck> CreateColumn(Vector3 position, Vector3 direction, int capacity, int spawnPosition)
    {
        return new TrucksColumn(position, direction, capacity, spawnPosition);
    }
}