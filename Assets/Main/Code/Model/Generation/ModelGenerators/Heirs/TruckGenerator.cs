public class TruckGenerator : ModelGenerator<Truck>
{
    public TruckGenerator(TruckFactory truckFactory,
                          ColorGenerator colorGenerator)
                   : base(truckFactory,
                          colorGenerator)
    {

    }
}