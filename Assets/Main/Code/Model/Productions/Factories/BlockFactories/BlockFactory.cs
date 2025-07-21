public abstract class BlockFactory<T> : ModelFactory<T> where T : Block
{
    protected BlockFactory(FactorySettings factorySettings)
    {
        InitializePool(factorySettings.InitialPoolSize,
                       factorySettings.MaxPoolCapacity);
    }
}