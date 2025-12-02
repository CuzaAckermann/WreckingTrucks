using System;

public class CartrigeBoxFillingCardCreator : FillingCardCreator<CartrigeBox>
{
    private CartrigeBoxFieldSettings _fieldSettings;

    public CartrigeBoxFillingCardCreator(ModelFactory<CartrigeBox> modelFactory)
                                  : base(modelFactory)
    {

    }

    public void SetCartrigeBoxFieldSettings(CartrigeBoxFieldSettings fieldSettings)
    {
        _fieldSettings = fieldSettings ?? throw new ArgumentNullException(nameof(fieldSettings));
    }

    protected override void FillFillingCard(FillingCard fillingCard)
    {
        int addedBoxes = 0;
        bool isFilled = false;
        int numberCurrentRow = 0;

        while (isFilled == false)
        {
            for (int layer = 0; layer < _fieldSettings.FieldSize.AmountLayers && isFilled == false; layer++)
            {
                for (int column = 0; column < _fieldSettings.FieldSize.AmountColumns; column++)
                {
                    if (addedBoxes >= _fieldSettings.AmountCartrigeBoxes)
                    {
                        isFilled = true;
                        break;
                    }

                    Model model = ModelFactory.Create();
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
    }
}