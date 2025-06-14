public class PurpleTruckFactory : TruckFactory
{
    public PurpleTruckFactory(GunFactory gunFactory, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, initialPoolSize, maxPoolCapacity) { }

    protected override Truck CreateModel()
    {
        return new PurpleTruck();
    }
}