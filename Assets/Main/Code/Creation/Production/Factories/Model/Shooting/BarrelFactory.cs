public class BarrelFactory : ModelFactory<Barrel>
{
    public BarrelFactory(FactorySettings factorySettings,
                         ModelSettings modelSettings)
                  : base(factorySettings,
                         modelSettings)
    {

    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new Barrel(positionManipulator,
                          MoverCreator.Create(positionManipulator),
                          new BarrelRotator(positionManipulator, ModelSettings.RotationSpeed));
    }
}