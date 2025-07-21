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
                                            BlockFieldSettings blockFieldSettings)
    {
        for (int layer = 0; layer < blockFieldSettings.Layers.Count; layer++)
        {
            BlockLayerSettings blockLayerSettings = blockFieldSettings.Layers[layer];

            for (int i = 0; i < blockLayerSettings.Rows.Count; i++)
            {
                BlockRowSettings row = blockLayerSettings.Rows[i];
                int currentColumn = 0;

                for (int j = 0; j < row.Sequences.Count; j++)
                {
                    BlockSequence sequence = row.Sequences[j];
                    Type modelType = TypeConverter.GetModelType(sequence.ColorType);

                    for (int k = 0; k < sequence.Amount; k++)
                    {
                        RecordPlaceableModel record = new RecordPlaceableModel(ModelProduction.CreateModel(modelType),
                                                                               layer,
                                                                               currentColumn++,
                                                                               i);

                        fillingCard.Add(record);
                    }
                }
            }
        }
    }
}