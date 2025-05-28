public abstract class TruckFactory : ModelFactory<Truck>
{
    public TruckFactory(int initialPoolSize, int maxPoolCapacity)
                 : base(initialPoolSize, maxPoolCapacity) { }

    protected abstract override Truck CreateModel();
}