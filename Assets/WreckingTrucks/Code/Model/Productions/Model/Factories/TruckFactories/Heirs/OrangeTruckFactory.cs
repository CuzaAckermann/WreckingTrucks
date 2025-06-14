public class OrangeTruckFactory : TruckFactory
{
    public OrangeTruckFactory(GunFactory gunFactory, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, initialPoolSize, maxPoolCapacity) { }

    protected override Truck CreateModel()
    {
        return new OrangeTruck();
    }
}