public class BlockGenerator : ModelGenerator<Block>
{
    public BlockGenerator(BlockFactory blockFactory,
                          ColorGenerator colorGenerator)
                   : base(blockFactory,
                          colorGenerator)
    {

    }
}