using System;

public class BlockFillingCardCreator
{
    private readonly BlockLayerSettingsConverter _blockLayerSettingsConverter;

    private BlockFieldSettings _blockFieldSettings;

    public BlockFillingCardCreator()
    {
        _blockLayerSettingsConverter = new BlockLayerSettingsConverter();
    }

    public void SetBlockFieldSettings(BlockFieldSettings blockFieldSettings)
    {
        _blockFieldSettings = blockFieldSettings ?? throw new ArgumentNullException(nameof(blockFieldSettings));
    }

    public FillingCard Create(FieldSize fieldSize)
    {
        FillingCard fillingCard = new FillingCard(fieldSize.AmountLayers,
                                                  fieldSize.AmountColumns,
                                                  fieldSize.AmountRows);

        ColorType[,,] colorTypes = _blockLayerSettingsConverter.Convert(_blockFieldSettings);

        for (int row = 0; row < colorTypes.GetLength(2); row++)
        {
            for (int layer = 0; layer < colorTypes.GetLength(0); layer++)
            {
                for (int column = 0; column < colorTypes.GetLength(1); column++)
                {
                    RecordPlaceableModel record = new RecordPlaceableModel(colorTypes[layer, column, row],
                                                                           layer,
                                                                           column,
                                                                           row);

                    fillingCard.Add(record);
                }
            }
        }

        return fillingCard;
    }
}