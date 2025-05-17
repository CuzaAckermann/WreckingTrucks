public class BlocksFactory
{
    private readonly Pool<Block> _blockPool;

    public BlocksFactory(int initialPoolSize, int maxPoolCapacity)
    {
        _blockPool = new Pool<Block>(CreateBlock,
                                     PrepareBlock,
                                     ResetBlock,
                                     DestroyBlock,
                                     initialPoolSize,
                                     maxPoolCapacity);
    }

    public Block GetBlock()
    {
        return _blockPool.GetElement();
    }

    public void Clear()
    {
        _blockPool.Clear();
    }

    #region Pool Logic
    private Block CreateBlock()
    {
        return new Block();
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