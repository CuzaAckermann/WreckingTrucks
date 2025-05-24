using System;

public class BlocksFactories
{
    private GreenBlockFactory _greenBlockFactory;
    private OrangeBlockFactory _orangeBlockFactory;
    private PurpleBlockFactory _purpleBlockFactory;

    public BlocksFactories(int initialPoolSize, int maxPoolCapacity)
    {
        _greenBlockFactory = new GreenBlockFactory(initialPoolSize, maxPoolCapacity);
        _orangeBlockFactory = new OrangeBlockFactory(initialPoolSize, maxPoolCapacity);
        _purpleBlockFactory = new PurpleBlockFactory(initialPoolSize, maxPoolCapacity);
    }

    public Block GetBlock(Block block)
    {
        switch (block)
        {
            case GreenBlock:
                return (Block)_greenBlockFactory.Create();

            case OrangeBlock:
                return (Block)_orangeBlockFactory.Create();

            case PurpleBlock:
                return (Block)_purpleBlockFactory.Create();

            default:
                throw new InvalidOperationException($"No factory for {nameof(block)}");
        }
    }
}