using System;

public abstract class TruckFactory : ModelFactory<Truck>
{
    protected readonly GunFactory GunFactory;
    protected readonly float ShotCooldown;

    public TruckFactory(GunFactory gunFactory, float shotCooldown, int initialPoolSize, int maxPoolCapacity)
    {
        if (shotCooldown <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        GunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        ShotCooldown = shotCooldown;
        InitializePool(initialPoolSize, maxPoolCapacity);
    }

    protected abstract override Truck CreateModel();
}