using System;

public class TruckFillingCardCreator : FillingCardCreator<Truck, TruckTypeConverter, TruckFieldSettings>
{
    private ModelTypeGenerator<Truck> _generator;

    public TruckFillingCardCreator(ModelProduction<Truck> modelProduction,
                                   TruckTypeConverter typeConverter)
                            : base(modelProduction,
                                   typeConverter)
    {

    }

    public void SetTruckTypeGenerator(ModelTypeGenerator<Truck> generator)
    {
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
    }

    protected override void FillFillingCard(FillingCard fillingCard,
                                            TruckFieldSettings truckFieldSettings)
    {
        for (int k = 0; k < truckFieldSettings.FieldSize.AmountLayers; k++)
        {
            for (int i = 0; i < truckFieldSettings.FieldSize.AmountRows; i++)
            {
                for (int j = 0; j < truckFieldSettings.FieldSize.AmountColumns; j++)
                {
                    fillingCard.Add(new RecordPlaceableModel(ModelProduction.CreateModel(_generator.Generate()),
                                                             k,
                                                             j,
                                                             i));
                }
            }
        }
    }
}