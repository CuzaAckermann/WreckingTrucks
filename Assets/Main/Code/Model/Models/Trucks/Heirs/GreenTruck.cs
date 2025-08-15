public class GreenTruck : Truck
{
    public GreenTruck(Gun gun,
                      Trunk trunk,
                      BlockTracker blockTracker,
                      Stopwatch stopwatch,
                      float shotCooldown)
               : base(gun,
                      trunk,
                      blockTracker,
                      stopwatch,
                      shotCooldown)
    {

    }
}