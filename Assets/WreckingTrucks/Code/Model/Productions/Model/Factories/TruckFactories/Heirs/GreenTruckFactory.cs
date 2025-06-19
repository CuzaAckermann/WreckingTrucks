public class GreenTruckFactory : TruckFactory
{
    public GreenTruckFactory(GunFactory gunFactory, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        GreenTruck greenTruck = new GreenTruck(GunFactory.Create());
        greenTruck.SetDestroyableType<GreenBlock>();

        return greenTruck;
    }
}