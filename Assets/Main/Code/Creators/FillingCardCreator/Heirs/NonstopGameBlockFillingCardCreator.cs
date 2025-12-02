using System;

public class NonstopGameBlockFillingCardCreator : FillingCardCreator<Block>
{
    private FieldSize _fieldSize;

    public NonstopGameBlockFillingCardCreator(ModelFactory<Block> modelFactory)
                                       : base(modelFactory)
    {

    }

    public void SetFieldSize(FieldSize fieldSize)
    {
        _fieldSize = fieldSize ?? throw new ArgumentNullException(nameof(fieldSize));
    }

    protected override void FillFillingCard(FillingCard fillingCard)
    {
        for (int row = 0; row < _fieldSize.AmountRows; row++)
        {
            for (int layer = 0; layer < _fieldSize.AmountLayers; layer++)
            {
                for (int column = 0; column < _fieldSize.AmountColumns; column++)
                {
                    Model model = ModelFactory.Create();

                    //model.SetColor(fieldSettings.ColorTypes[layer, row, column]);

                    RecordPlaceableModel record = new RecordPlaceableModel(model,
                                                                           layer,
                                                                           column,
                                                                           row);

                    fillingCard.Add(record);
                }
            }
        }
    }
}