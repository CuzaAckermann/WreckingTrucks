public class GunFactory : ModelFactory<Gun>
{
    public GunFactory(FactorySettings factorySettings)
    {
        InitializePool(factorySettings.InitialPoolSize,
                       factorySettings.MaxPoolCapacity);
    }

    protected override Gun CreateElement()
    {
        return new Gun(10);
    }
}