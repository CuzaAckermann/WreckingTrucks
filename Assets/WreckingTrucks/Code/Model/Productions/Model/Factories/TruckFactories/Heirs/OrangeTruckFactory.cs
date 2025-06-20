public class OrangeTruckFactory : TruckFactory
{
    public OrangeTruckFactory(GunFactory gunFactory, float shotCooldown, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, shotCooldown, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        OrangeTruck orangeTruck = new OrangeTruck(GunFactory.Create(), ShotCooldown);
        orangeTruck.SetDestroyableType<OrangeBlock>();

        return orangeTruck;
    }
}