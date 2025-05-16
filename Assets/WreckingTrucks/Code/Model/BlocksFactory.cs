using System;
using System.Collections.Generic;

public class BlocksFactory
{
    private readonly Stack<Block> _blockPool;
    private readonly int _maxPoolCapacity;

    public BlocksFactory(int initialPoolSize, int maxPoolCapacity)
    {
        if (initialPoolSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialPoolSize), $"{nameof(initialPoolSize)} cannot be negative");
        }

        if (maxPoolCapacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxPoolCapacity), $"{nameof(maxPoolCapacity)} must be positive");
        }

        if (initialPoolSize > maxPoolCapacity)
        {
            throw new ArgumentException($"{nameof(initialPoolSize)} cannot exceed {nameof(maxPoolCapacity)}");
        }

        _blockPool = new Stack<Block>(initialPoolSize);
        _maxPoolCapacity = maxPoolCapacity;

        PrewarmPool(initialPoolSize);
    }

    public Block GetBlock()
    {
        Block block;

        if (_blockPool.Count > 0)
        {
            block = _blockPool.Pop();
        }
        else
        {
            block = InstantiateBlock();
        }

        block.Destroyed += ReturnBlockToPool;

        return block;
    }

    private void PrewarmPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _blockPool.Push(InstantiateBlock());
        }
    }

    private void ReturnBlockToPool(Block block)
    {
        if (block == null)
        {
            return;
        }

        block.Destroyed -= ReturnBlockToPool;

        if (_blockPool.Count < _maxPoolCapacity)
        {
            _blockPool.Push(block);
        }
    }

    private Block InstantiateBlock()
    {
        return new Block();
    }
}