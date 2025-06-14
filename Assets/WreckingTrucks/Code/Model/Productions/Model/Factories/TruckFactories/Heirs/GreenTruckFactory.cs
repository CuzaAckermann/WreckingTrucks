public class GreenTruckFactory : TruckFactory
{
    public GreenTruckFactory(GunFactory gunFactory, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, initialPoolSize, maxPoolCapacity) { }

    protected override Truck CreateModel()
    {
        //GunFactory.Create();

        return new GreenTruck();
    }
}