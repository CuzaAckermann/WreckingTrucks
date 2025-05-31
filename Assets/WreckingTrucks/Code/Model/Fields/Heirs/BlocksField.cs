using UnityEngine;

public class BlocksField : Field<Block>
{
    public BlocksField(Vector3 position, Vector3 columnDirection, Vector3 rowDirection,
                       int amountColumns, int capacityColumn, int spawnPosition,
                       Mover<Block> modelsMover)
                : base(position, columnDirection, rowDirection,
                       amountColumns, capacityColumn, spawnPosition,
                       modelsMover)
    {

    }

    protected override Column<Block> CreateColumn(Vector3 position,
                                                  Vector3 direction,
                                                  int capacity,
                                                  int spawnPosition)
    {
        return new BlocksColumn(position, direction, capacity, spawnPosition);
    }
}