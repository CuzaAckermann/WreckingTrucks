public abstract class BlockFactory : ModelFactory<Block>
{
    protected BlockFactory(int initialPoolSize, int maxPoolCapacity)
                    : base(initialPoolSize, maxPoolCapacity) { }

    protected abstract override Block CreateModel();
}