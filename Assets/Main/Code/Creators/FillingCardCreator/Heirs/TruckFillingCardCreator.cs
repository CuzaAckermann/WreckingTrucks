using System;
using System.Collections.Generic;

public class TruckFillingCardCreator : FillingCardCreator<Truck>
{
    private readonly TruckGenerator _truckGenerator;

    public TruckFillingCardCreator(ModelFactory<Truck> modelFactory, TruckGenerator truckGenerator)
                            : base(modelFactory)
    {
        _truckGenerator = truckGenerator ?? throw new ArgumentNullException(nameof(truckGenerator));
    }

    public void SetColorTypes(IReadOnlyList<ColorType> colorTypes)
    {
        _truckGenerator.SetColorTypes(colorTypes);
    }

    protected override void FillFillingCard(FillingCard fillingCard)
    {
        for (int layer = 0; layer < fillingCard.AmountLayers; layer++)
        {
            for (int row = 0; row < fillingCard.AmountRows; row++)
            {
                for (int column = 0; column < fillingCard.AmountColumns; column++)
                {
                    fillingCard.Add(new RecordPlaceableModel(_truckGenerator.Generate(),
                                                             layer,
                                                             column,
                                                             row));
                }
            }
        }
    }
}