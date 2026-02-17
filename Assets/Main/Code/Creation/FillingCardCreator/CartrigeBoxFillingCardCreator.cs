using System;

public class CartrigeBoxFillingCardCreator
{
    public FillingCard Create(FieldSize fieldSize, int amountCartrigeBoxes)
    {
        FillingCard fillingCard = new FillingCard(fieldSize.AmountLayers,
                                                  fieldSize.AmountColumns,
                                                  fieldSize.AmountRows);

        int addedBoxes = 0;
        bool isFilled = false;
        int numberCurrentRow = 0;

        while (isFilled == false)
        {
            for (int layer = 0; layer < fieldSize.AmountLayers && isFilled == false; layer++)
            {
                for (int column = 0; column < fieldSize.AmountColumns; column++)
                {
                    if (addedBoxes >= amountCartrigeBoxes)
                    {
                        isFilled = true;
                        break;
                    }

                    fillingCard.Add(new RecordPlaceableModel(ColorType.Gray,
                                                             layer,
                                                             column,
                                                             numberCurrentRow));

                    addedBoxes++;
                }
            }

            numberCurrentRow++;
        }

        return fillingCard;
    }
}