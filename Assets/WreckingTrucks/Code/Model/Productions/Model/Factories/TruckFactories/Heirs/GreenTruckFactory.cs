using UnityEngine;

public class GreenTruckFactory : TruckFactory
{
    public GreenTruckFactory(GunFactory gunFactory, float shotCooldown, Vector3 localPositionGun, int initialPoolSize, int maxPoolCapacity)
                      : base(gunFactory, shotCooldown, localPositionGun, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        GreenTruck greenTruck = new GreenTruck(GunFactory.Create(), ShotCooldown, LocalPositionGun);
        greenTruck.SetDestroyableType<GreenBlock>();

        return greenTruck;
    }
}