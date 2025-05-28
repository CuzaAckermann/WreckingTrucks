public class PurpleTruckFactory : TruckFactory
{
    public PurpleTruckFactory(int initialPoolSize, int maxPoolCapacity)
                       : base(initialPoolSize, maxPoolCapacity) { }

    protected override Truck CreateModel()
    {
        return new PurpleTruck();
    }
}