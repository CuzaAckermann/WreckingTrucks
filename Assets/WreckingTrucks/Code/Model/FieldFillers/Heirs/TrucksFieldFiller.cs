public class TrucksFieldFiller : ModelsFieldFiller<Truck, TruckFactory>
{
    public TrucksFieldFiller(ModelsProduction<Truck, TruckFactory> modelsProduction, Field<Truck> modelsField, int startCapacityQueue)
                      : base(modelsProduction, modelsField, startCapacityQueue)
    {

    }
}