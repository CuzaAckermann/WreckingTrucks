using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockFillingCardCreator
{
    private readonly BlockFactory _blockFactory;
    private readonly BlockLayerSettingsConverter _blockLayerSettingsConverter;

    private BlockFieldSettings _blockFieldSettings;

    public BlockFillingCardCreator(BlockFactory blockFactory)
    {
        _blockFactory = blockFactory ?? throw new ArgumentNullException(nameof(blockFactory));

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
                    Model model = _blockFactory.Create();
                    model.SetColor(colorTypes[layer, column, row]);
                    RecordPlaceableModel record = new RecordPlaceableModel(model,
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