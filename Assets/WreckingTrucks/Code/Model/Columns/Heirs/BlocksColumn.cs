using UnityEngine;

public class BlocksColumn : Column<Block>
{
    public BlocksColumn(Vector3 position, Vector3 direction, int capacity, int spawnPosition)
                 : base(position, direction, capacity, spawnPosition)
    {

    }
}