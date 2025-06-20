public class GreenTruckFactory : TruckFactory
{
    public GreenTruckFactory(GunFactory gunFactory, float shotCooldown, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, shotCooldown, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        GreenTruck greenTruck = new GreenTruck(GunFactory.Create(), ShotCooldown);
        greenTruck.SetDestroyableType<GreenBlock>();

        return greenTruck;
    }
}