public class BlockGenerator : ModelGenerator<Block>
{
    public BlockGenerator(ModelFactory<Block> modelFactory,
                          ModelProbabilitySettings modelProbabilitySettings,
                          float minAmountProbabilityReduction,
                          float maxAmountProbabilityReduction)
                   : base(modelFactory,
                          modelProbabilitySettings,
                          minAmountProbabilityReduction,
                          maxAmountProbabilityReduction)
    {

    }
}