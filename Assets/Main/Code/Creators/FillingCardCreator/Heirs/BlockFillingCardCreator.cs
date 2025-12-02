using System;
using System.Collections.Generic;

public class BlockFillingCardCreator : FillingCardCreator<Block>
{
    private IReadOnlyList<BlockLayerSettings> _blockLayerSettings;

    public BlockFillingCardCreator(ModelFactory<Block> modelFactory)
                            : base(modelFactory)
    {

    }

    public void SetBlockLayerSettings(IReadOnlyList<BlockLayerSettings> blockLayerSettings)
    {
        _blockLayerSettings = blockLayerSettings ?? throw new ArgumentNullException(nameof(blockLayerSettings));
    }

    protected override void FillFillingCard(FillingCard fillingCard)
    {
        //FillByLayers(fillingCard);

        FillByRows(fillingCard);
    }

    // метод дл€ заполнени€ пол€ по—Ћќ…но
    //private void FillByLayers(FillingCard fillingCard)
    //{
    //    for (int layer = 0; layer < _blockLayerSettings.Count; layer++)
    //    {
    //        BlockLayerSettings blockLayerSettings = _blockLayerSettings[layer];

    //        for (int row = 0; row < blockLayerSettings.Rows.Count; row++)
    //        {
    //            BlockRowSettings currentRow = blockLayerSettings.Rows[row];
    //            int currentColumn = 0;

    //            for (int sequence = 0; sequence < currentRow.Sequences.Count; sequence++)
    //            {
    //                BlockSequence currentSequence = currentRow.Sequences[sequence];
    //                Model model = ModelFactory.Create();
    //                model.SetColor(currentSequence.ColorType);

    //                for (int element = 0; element < currentSequence.Amount; element++)
    //                {
    //                    RecordPlaceableModel record = new RecordPlaceableModel(model,
    //                                                                           layer,
    //                                                                           currentColumn++,
    //                                                                           row);

    //                    fillingCard.Add(record);
    //                }
    //            }
    //        }
    //    }
    //}


    // метод дл€ заполнени€ пол€ по–яƒово
    private void FillByRows(FillingCard fillingCard)
    {
        int layer = 0;

        for (int row = 0; row < _blockLayerSettings[layer].Rows.Count; row++)
        {
            for (; layer < _blockLayerSettings.Count; layer++)
            {
                BlockRowSettings currentRow = _blockLayerSettings[layer].Rows[row];
                int currentColumn = 0;

                for (int sequence = 0; sequence < currentRow.Sequences.Count; sequence++)
                {
                    BlockSequence currentSequence = currentRow.Sequences[sequence];
                    
                    for (int element = 0; element < currentSequence.Amount; element++)
                    {
                        Model model = ModelFactory.Create();
                        model.SetColor(currentSequence.ColorType);
                        RecordPlaceableModel record = new RecordPlaceableModel(model,
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