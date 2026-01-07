using System;
using System.Collections.Generic;

public class TruckFillingCardCreator
{
    private readonly ModelColorGenerator _modelColorGenerator;

    public TruckFillingCardCreator(ModelColorGenerator truckGenerator)
    {
        _modelColorGenerator = truckGenerator ?? throw new ArgumentNullException(nameof(truckGenerator));
    }

    public void SetColorTypes(IReadOnlyList<ColorType> colorTypes)
    {
        _modelColorGenerator.SetColorTypes(colorTypes);
    }

    public FillingCard Create(FieldSize fieldSize)
    {
        FillingCard fillingCard = new FillingCard(fieldSize.AmountLayers,
                                                  fieldSize.AmountColumns,
                                                  fieldSize.AmountRows);

        for (int layer = 0; layer < fieldSize.AmountLayers; layer++)
        {
            for (int row = 0; row < fieldSize.AmountRows; row++)
            {
                for (int column = 0; column < fieldSize.AmountColumns; column++)
                {
                    fillingCard.Add(new RecordPlaceableModel(_modelColorGenerator.Generate(),
                                                             layer,
                                                             column,
                                                             row));
                }
            }
        }

        return fillingCard;
    }
}