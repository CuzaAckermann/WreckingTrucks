public abstract class BlockFactory<T> : ModelFactory where T : Block, new()
{
    protected BlockFactory(int initialPoolSize, int maxPoolCapacity)
                    : base(initialPoolSize, maxPoolCapacity) { }

    protected sealed override Model CreateModel()
    {
        return CreateBlock();
    }

    protected abstract T CreateBlock();
}