public class TruckGenerator : ModelGenerator<Truck>
{
    public TruckGenerator(ModelFactory<Truck> modelFactory,
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