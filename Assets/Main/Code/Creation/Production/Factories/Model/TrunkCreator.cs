public class TrunkCreator : ModelFactory<Trunk>
{
    public TrunkCreator(FactorySettings factorySettings, ModelSettings modelSettings) : base(factorySettings, modelSettings)
    {

    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new Trunk(positionManipulator,
                         MoverCreator.Create(positionManipulator),
                         RotatorCreator.Create(positionManipulator));
    }
}