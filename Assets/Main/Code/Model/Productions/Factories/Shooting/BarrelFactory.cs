public class BarrelFactory : ModelFactory<Barrel>
{
    public BarrelFactory(FactorySettings factorySettings,
                         ModelSettings modelSettings)
                  : base(factorySettings,
                         modelSettings)
    {
        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    protected override Barrel CreateElement()
    {
        PositionManipulator positionManipulator = new PositionManipulator();

        return new Barrel(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          new BarrelRotator(positionManipulator, ModelSettings.RotationSpeed));
    }
}