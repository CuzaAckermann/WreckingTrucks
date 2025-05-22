public abstract class BlockFactory<T> : IFactory<Block> where T : Block, new()
{
    private readonly Pool<Block> _blockPool;

    public BlockFactory(int initialPoolSize, int maxPoolCapacity)
    {
        _blockPool = new Pool<Block>(CreateBlock,
                                     PrepareBlock,
                                     ResetBlock,
                                     DestroyBlock,
                                     initialPoolSize,
                                     maxPoolCapacity);
    }

    public Block Create()
    {
        return _blockPool.GetElement();
    }

    public void Clear()
    {
        _blockPool.Clear();
    }

    #region Pool Callback
    private Block CreateBlock()
    {
        return new T();
    }

    private void PrepareBlock(Block block)
    {
        block.Destroyed += ReturnBlock;
    }

    private void ResetBlock(Block block)
    {
        block.Destroyed -= ReturnBlock;
    }

    private void DestroyBlock(Block block)
    {
        block?.Destroy();
    }
    
    private void ReturnBlock(Block block)
    {
        if (block != null)
        {
            _blockPool.Release(block);
        }
    }
    #endregion
}