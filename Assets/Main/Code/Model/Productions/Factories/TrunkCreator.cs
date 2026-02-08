public class TrunkCreator : ModelFactory<Trunk>
{
    public TrunkCreator(FactorySettings factorySettings, ModelSettings modelSettings) : base(factorySettings, modelSettings)
    {
        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    protected override Trunk CreateElement()
    {
        PositionManipulator positionManipulator = new PositionManipulator();

        return new Trunk(positionManipulator,
                         MoverCreator.Create(positionManipulator),
                         RotatorCreator.Create(positionManipulator));
    }
}