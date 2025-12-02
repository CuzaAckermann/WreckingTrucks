public class BulletFactory : ModelFactory<Bullet>
{
    public BulletFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                  : base(factorySettings, modelSettings)
    {
        InitializePool(factorySettings.InitialPoolSize,
                       factorySettings.MaxPoolCapacity);
    }

    protected override Bullet CreateElement()
    {
        return new Bullet(ModelSettings.Movespeed, ModelSettings.Rotatespeed);
    }
}