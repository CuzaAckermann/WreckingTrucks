public class CartrigeBoxGenerator : ModelGenerator<CartrigeBox>
{
    public CartrigeBoxGenerator(ModelFactory<CartrigeBox> modelFactory,
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