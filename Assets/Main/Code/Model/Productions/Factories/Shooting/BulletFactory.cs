public class BulletFactory : ModelFactory<Bullet>
{
    public BulletFactory(FactorySettings factorySettings)
    {
        InitializePool(factorySettings.InitialPoolSize,
                       factorySettings.MaxPoolCapacity);
    }

    protected override Bullet CreateElement()
    {
        return new Bullet();
    }
}