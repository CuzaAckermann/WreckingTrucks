public class OrangeTruckFactory : TruckFactory
{
    public OrangeTruckFactory(GunFactory gunFactory, int initialPoolSize, int maxPoolCapacity)
                       : base(gunFactory, initialPoolSize, maxPoolCapacity)
    {

    }

    protected override Truck CreateModel()
    {
        OrangeTruck orangeTruck = new OrangeTruck(GunFactory.Create());
        orangeTruck.SetDestroyableType<OrangeBlock>();

        return orangeTruck;
    }
}