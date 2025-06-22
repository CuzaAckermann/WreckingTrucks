using UnityEngine;

public class OrangeTruckFactory : TruckFactory
{
    public OrangeTruckFactory(GunFactory gunFactory, float shotCooldown, Vector3 localPosition, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, shotCooldown, localPosition, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        OrangeTruck orangeTruck = new OrangeTruck(GunFactory.Create(), ShotCooldown, LocalPositionGun);
        orangeTruck.SetDestroyableType<OrangeBlock>();

        return orangeTruck;
    }
}