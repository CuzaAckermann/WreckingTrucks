public class BlocksProduction : ModelsProduction
{
    public Block GetBlock(Block block)
    {
        return (Block)CreateModel(block);
    }
}