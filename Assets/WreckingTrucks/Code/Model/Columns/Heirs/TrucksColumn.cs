using UnityEngine;

public class TrucksColumn : Column<Truck>
{
    public TrucksColumn(Vector3 position, Vector3 direction, int capacity, int spawnPosition)
                 : base(position, direction, capacity, spawnPosition)
    {

    }
}