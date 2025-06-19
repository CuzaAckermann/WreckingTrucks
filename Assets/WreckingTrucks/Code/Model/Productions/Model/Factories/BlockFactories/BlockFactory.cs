public abstract class BlockFactory : ModelFactory<Block>
{
    protected BlockFactory(int initialPoolSize, int maxPoolCapacity)
    {
        InitializePool(initialPoolSize, maxPoolCapacity);
    }

    protected abstract override Block CreateModel();
}