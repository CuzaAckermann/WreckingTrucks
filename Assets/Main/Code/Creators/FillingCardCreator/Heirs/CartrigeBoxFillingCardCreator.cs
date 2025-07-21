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
        for (int i = 0; i < fieldSettings.FieldSize.AmountRows; i++)
        {
            for (int k = 0; k < fieldSettings.FieldSize.AmountLayers; k++)
            {
                for (int j = 0; j < fieldSettings.FieldSize.AmountColumns; j++)
                {
                    fillingCard.Add(new RecordPlaceableModel(ModelProduction.CreateModel(typeof(CartrigeBox)),
                                                             k,
                                                             j,
                                                             i));

                }
            }
        }
    }
}