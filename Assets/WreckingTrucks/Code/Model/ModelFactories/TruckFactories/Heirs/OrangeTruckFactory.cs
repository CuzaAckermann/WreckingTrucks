public class OrangeTruckFactory : TruckFactory
{
    public OrangeTruckFactory(int initialPoolSize, int maxPoolCapacity)
                       : base(initialPoolSize, maxPoolCapacity) { }

    protected override Truck CreateModel()
    {
        return new OrangeTruck();
    }
}