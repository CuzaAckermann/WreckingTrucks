using System;

public class BlocksFactories : IBlockVisitor
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

    public GreenBlock Visit(GreenBlock greenBlock)
    {
        return new GreenBlock();

        //return _greenBlockFactory.Create();
    }

    public OrangeBlock Visit(OrangeBlock orangeBlock)
    {
        return new OrangeBlock();

        //return _orangeBlockFactory.Create();
    }

    public PurpleBlock Visit(PurpleBlock purpleBlock)
    {
        return new PurpleBlock();

        //return _purpleBlockFactory.Create();
    }

    public Block GetBlock(Block block)
    {
        switch (block)
        {
            case GreenBlock:
                return _greenBlockFactory.Create();

            case OrangeBlock:
                return _orangeBlockFactory.Create();

            case PurpleBlock:
                return _purpleBlockFactory.Create();

            default:
                throw new InvalidOperationException($"No factory for {nameof(block)}");
        }
    }
}