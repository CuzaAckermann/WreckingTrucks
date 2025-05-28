public class GreenTruckFactory : TruckFactory
{
    public GreenTruckFactory(int initialPoolSize, int maxPoolCapacity)
                      : base(initialPoolSize, maxPoolCapacity) { }

    protected override Truck CreateModel()
    {
        return new GreenTruck();
    }
}