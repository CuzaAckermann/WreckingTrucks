using System;
using UnityEngine;

public abstract class TruckFactory : ModelFactory<Truck>
{
    protected readonly GunFactory GunFactory;
    protected readonly float ShotCooldown;
    protected readonly Vector3 LocalPositionGun;

    public TruckFactory(GunFactory gunFactory, float shotCooldown, Vector3 localPositionGun, int initialPoolSize, int maxPoolCapacity)
    {
        if (shotCooldown <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        GunFactory = gunFactory ?? throw new ArgumentNullException(nameof(gunFactory));
        ShotCooldown = shotCooldown;
        LocalPositionGun = localPositionGun;
        InitializePool(initialPoolSize, maxPoolCapacity);
    }

    protected abstract override Truck CreateModel();
}