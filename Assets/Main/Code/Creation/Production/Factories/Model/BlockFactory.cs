public class BlockFactory : ModelFactory<Block>
{
    public BlockFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                 : base(factorySettings, modelSettings)
    {

    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new Block(positionManipulator,
                         MoverCreator.Create(positionManipulator),
                         RotatorCreator.Create(positionManipulator));
    }
}