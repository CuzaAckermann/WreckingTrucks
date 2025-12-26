using System;

public class CartrigeBoxFillingCardCreator
{
    private readonly CartrigeBoxFactory _cartrigeBoxFactory;

    public CartrigeBoxFillingCardCreator(CartrigeBoxFactory cartrigeBoxFactory)
    {
        _cartrigeBoxFactory = cartrigeBoxFactory ?? throw new ArgumentNullException(nameof(cartrigeBoxFactory));
    }

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

                    Model model = _cartrigeBoxFactory.Create();
                    model.SetColor(ColorType.Gray);
                    fillingCard.Add(new RecordPlaceableModel(model,
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