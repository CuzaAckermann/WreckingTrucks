using UnityEngine;

public class OrangeTruck : Truck
{
    public OrangeTruck(Gun gun,
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