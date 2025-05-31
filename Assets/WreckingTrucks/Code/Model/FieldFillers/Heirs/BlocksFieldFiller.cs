public class BlocksFieldFiller : ModelsFieldFiller<Block, BlockFactory>
{
    public BlocksFieldFiller(ModelsProduction<Block, BlockFactory> modelsProduction, Field<Block> modelsField, int startCapacityQueue)
                      : base(modelsProduction, modelsField, startCapacityQueue)
    {

    }
}