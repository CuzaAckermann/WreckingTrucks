using UnityEngine;

public class PurpleTruckFactory : TruckFactory
{
    public PurpleTruckFactory(GunFactory gunFactory, float shotCooldown, Vector3 localPosition, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, shotCooldown, localPosition, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        PurpleTruck purpleTruck = new PurpleTruck(GunFactory.Create(), ShotCooldown, LocalPositionGun);
        purpleTruck.SetDestroyableType<PurpleBlock>();

        return purpleTruck;
    }
}