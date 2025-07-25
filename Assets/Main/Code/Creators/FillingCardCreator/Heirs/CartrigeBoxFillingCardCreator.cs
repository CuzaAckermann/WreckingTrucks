public class CartrigeBoxFillingCardCreator : FillingCardCreator<CartrigeBox, ITypeConverter, CartrigeBoxFieldSettings>
{
    public CartrigeBoxFillingCardCreator(ModelProduction<CartrigeBox> modelProduction,
                                         ITypeConverter typeConverter)
                                  : base(modelProduction,
                                         typeConverter)
    {

    }

    protected override void FillFillingCard(FillingCard fillingCard, CartrigeBoxFieldSettings fieldSettings)
    {
        int addedBoxes = 0;
        bool isFilled = false;
        int numberCurrentRow = 0;

        while (isFilled == false)
        {
            for (int layer = 0; layer < fieldSettings.FieldSize.AmountLayers && isFilled == false; layer++)
            {
                for (int column = 0; column < fieldSettings.FieldSize.AmountColumns; column++)
                {
                    if (addedBoxes >= fieldSettings.AmountCartrigeBoxes)
                    {
                        isFilled = true;
                        break;
                    }

                    fillingCard.Add(new RecordPlaceableModel(ModelProduction.CreateModel(typeof(CartrigeBox)),
                                                             layer,
                                                             column,
                                                             numberCurrentRow));
                    addedBoxes++;
                }
            }

            numberCurrentRow++;
        }
    }
}