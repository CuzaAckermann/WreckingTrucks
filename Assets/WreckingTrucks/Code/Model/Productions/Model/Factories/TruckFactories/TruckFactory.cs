using System;

public abstract class TruckFactory : ModelFactory<Truck>
{
    protected readonly GunFactory GunFactory;

    public TruckFactory(GunFactory gunFactory, int initialPoolSize, int maxPoolCapacity)
    {
        GunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        InitializePool(initialPoolSize, maxPoolCapacity);
    }

    protected abstract override Truck CreateModel();
}