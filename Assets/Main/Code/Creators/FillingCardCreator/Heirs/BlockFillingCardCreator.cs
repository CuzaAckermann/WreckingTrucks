using System;
using System.Collections.Generic;

public class BlockFillingCardCreator
{
    private readonly BlockFactory _blockFactory;

    private IReadOnlyList<BlockLayerSettings> _blockLayerSettings;

    public BlockFillingCardCreator(BlockFactory blockFactory)
    {
        _blockFactory = blockFactory ?? throw new ArgumentNullException(nameof(blockFactory));
    }

    public void SetBlockLayerSettings(IReadOnlyList<BlockLayerSettings> blockLayerSettings)
    {
        _blockLayerSettings = blockLayerSettings ?? throw new ArgumentNullException(nameof(blockLayerSettings));
    }

    public FillingCard Create(FieldSize fieldSize)
    {
        FillingCard fillingCard = new FillingCard(fieldSize.AmountLayers,
                                                  fieldSize.AmountColumns,
                                                  fieldSize.AmountRows);

        // по–яƒовое заполнение

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
                        Model model = _blockFactory.Create();
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

        return fillingCard;
    }
}