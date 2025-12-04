public class BlockFactory : ModelFactory<Block>
{
    public BlockFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                 : base(factorySettings, modelSettings)
    {
        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    protected override Block CreateElement()
    {
        return new Block(ModelSettings.Movespeed, ModelSettings.Rotatespeed);
    }
}