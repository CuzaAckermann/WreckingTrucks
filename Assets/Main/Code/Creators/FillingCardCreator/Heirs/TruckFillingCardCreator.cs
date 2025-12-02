using System;
using System.Collections.Generic;

public class TruckFillingCardCreator : FillingCardCreator<Truck>
{
    private readonly TruckGenerator _truckGenerator;

    private FieldSize _fieldSize;

    public TruckFillingCardCreator(ModelFactory<Truck> modelFactory, TruckGenerator truckGenerator)
                            : base(modelFactory)
    {
        _truckGenerator = truckGenerator ?? throw new ArgumentNullException(nameof(truckGenerator));
    }

    public void SetFieldSize(FieldSize fieldSize)
    {
        _fieldSize = fieldSize;
    }

    public void SetColorTypes(IReadOnlyList<ColorType> colorTypes)
    {
        _truckGenerator.SetColorTypes(colorTypes);
    }

    protected override void FillFillingCard(FillingCard fillingCard)
    {
        for (int layer = 0; layer < _fieldSize.AmountLayers; layer++)
        {
            for (int row = 0; row < _fieldSize.AmountRows; row++)
            {
                for (int column = 0; column < _fieldSize.AmountColumns; column++)
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