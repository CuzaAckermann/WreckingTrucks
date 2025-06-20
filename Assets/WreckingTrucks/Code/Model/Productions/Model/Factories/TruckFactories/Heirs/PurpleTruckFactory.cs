public class PurpleTruckFactory : TruckFactory
{
    public PurpleTruckFactory(GunFactory gunFactory, float shotCooldown, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, shotCooldown, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        PurpleTruck purpleTruck = new PurpleTruck(GunFactory.Create(), ShotCooldown);
        purpleTruck.SetDestroyableType<PurpleBlock>();

        return purpleTruck;
    }
}