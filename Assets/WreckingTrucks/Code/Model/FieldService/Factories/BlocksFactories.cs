using System;
using System.Collections.Generic;

public class BlocksFactories
{
    private readonly Dictionary<BlockType, IBlockFactory> _factories;

    public BlocksFactories()
    {
        _factories = new Dictionary<BlockType, IBlockFactory>();
    }

    public void RegisterFactory(BlockType blockType, IBlockFactory factory)
    {
        if (_factories.ContainsKey(blockType))
        {
            throw new ArgumentException($"{nameof(factory)} with {nameof(blockType)} already exists");
        }

        _factories[blockType] = factory;
    }

    public Block GetBlock(BlockType blockType)
    {
        if (_factories.TryGetValue(blockType, out var factory))
        {
            return factory.GetBlock();
        }

        throw new KeyNotFoundException($"No factory for {blockType}");
    }
}