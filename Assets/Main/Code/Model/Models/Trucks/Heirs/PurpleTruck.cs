using UnityEngine;

public class PurpleTruck : Truck
{
    public PurpleTruck(Gun gun,
                       Trunk trunk,
                       BlockTracker blockTracker,
                       Stopwatch stopwatch,
                       float shotCooldown,
                       Vector3 gunPosition,
                       Vector3 trunkPosition)
                : base(gun,
                       trunk,
                       blockTracker,
                       stopwatch,
                       shotCooldown,
                       gunPosition,
                       trunkPosition)
    {

    }
}