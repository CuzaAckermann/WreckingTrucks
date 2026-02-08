public class BulletFactory : ModelFactory<Bullet>
{
    public BulletFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                  : base(factorySettings, modelSettings)
    {
        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    protected override Bullet CreateElement()
    {
        PositionManipulator positionManipulator = new PositionManipulator();

        return new Bullet(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          RotatorCreator.Create(positionManipulator));
    }
}