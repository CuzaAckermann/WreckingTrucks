public class BulletFactory : ModelFactory<Bullet>
{
    public BulletFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                  : base(factorySettings, modelSettings)
    {

    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new Bullet(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          RotatorCreator.Create(positionManipulator));
    }
}