using System;

public abstract class TruckFactory : ModelFactory<Truck>
{
    public TruckFactory(GunFactory gunFactory, int initialPoolSize, int maxPoolCapacity)
                 : base(initialPoolSize, maxPoolCapacity)
    {
    }

    protected abstract override Truck CreateModel();
}