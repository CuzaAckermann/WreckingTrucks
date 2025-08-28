using System;

public class BlockFillingCardCreator : FillingCardCreator<Block, BlockTypeConverter, BlockFieldSettings>
{
    public BlockFillingCardCreator(ModelProduction<Block> modelProduction,
                                   BlockTypeConverter typeConverter)
                            : base(modelProduction,
                                   typeConverter)
    {

    }

    protected override void FillFillingCard(FillingCard fillingCard,
                                            BlockFieldSettings fieldSettings)
    {
        //FillByLayers(fillingCard, fieldSettings);

        FillByRows(fillingCard, fieldSettings);
    }

    private void FillByLayers(FillingCard fillingCard,
                              BlockFieldSettings fieldSettings)
    {
        for (int layer = 0; layer < fieldSettings.Layers.Count; layer++)
        {
            BlockLayerSettings blockLayerSettings = fieldSettings.Layers[layer];

            for (int row = 0; row < blockLayerSettings.Rows.Count; row++)
            {
                BlockRowSettings currentRow = blockLayerSettings.Rows[row];
                int currentColumn = 0;

                for (int sequence = 0; sequence < currentRow.Sequences.Count; sequence++)
                {
                    BlockSequence currentSequence = currentRow.Sequences[sequence];
                    Type modelType = TypeConverter.GetModelType(currentSequence.ColorType);

                    for (int element = 0; element < currentSequence.Amount; element++)
                    {
                        RecordPlaceableModel record = new RecordPlaceableModel(ModelProduction.CreateModel(modelType),
                                                                               layer,
                                                                               currentColumn++,
                                                                               row);

                        fillingCard.Add(record);
                    }
                }
            }
        }
    }

    private void FillByRows(FillingCard fillingCard,
                            BlockFieldSettings fieldSettings)
    {
        int layer = 0;

        for (int row = 0; row < fieldSettings.Layers[layer].Rows.Count; row++)
        {
            for (; layer < fieldSettings.Layers.Count; layer++)
            {
                BlockRowSettings currentRow = fieldSettings.Layers[layer].Rows[row];
                int currentColumn = 0;

                for (int sequence = 0; sequence < currentRow.Sequences.Count; sequence++)
                {
                    BlockSequence currentSequence = currentRow.Sequences[sequence];
                    Type modelType = TypeConverter.GetModelType(currentSequence.ColorType);

                    for (int element = 0; element < currentSequence.Amount; element++)
                    {
                        RecordPlaceableModel record = new RecordPlaceableModel(ModelProduction.CreateModel(modelType),
                                                                               layer,
                                                                               currentColumn++,
                                                                               row);

                        fillingCard.Add(record);
                    }
                }
            }

            layer = 0;
        }
    }
}