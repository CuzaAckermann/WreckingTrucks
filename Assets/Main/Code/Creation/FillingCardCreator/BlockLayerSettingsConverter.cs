using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockLayerSettingsConverter
{
    public ColorType[,,] Convert(BlockFieldSettings blockFieldSettings)
    {
        if (blockFieldSettings == null)
        {
            throw new ArgumentNullException(nameof(blockFieldSettings));
        }

        if (blockFieldSettings.FieldSize == null)
        {
            throw new ArgumentNullException(nameof(blockFieldSettings.FieldSize));
        }

        ColorType[,,] colorTypes = new ColorType[blockFieldSettings.FieldSize.AmountLayers,
                                                 blockFieldSettings.FieldSize.AmountColumns,
                                                 blockFieldSettings.FieldSize.AmountRows];

        for (int layer = 0; layer < blockFieldSettings.Layers.Count; layer++)
        {
            for (int row = 0; row < blockFieldSettings.Layers[layer].Rows.Count; row++)
            {
                BlockRowSettings currentRow = blockFieldSettings.Layers[layer].Rows[row];
                int currentColumn = 0;

                for (int sequence = 0; sequence < currentRow.Sequences.Count; sequence++)
                {
                    BlockSequence currentSequence = currentRow.Sequences[sequence];

                    for (int element = 0; element < currentSequence.Amount; element++)
                    {
                        colorTypes[layer, currentColumn++, row] = currentSequence.ColorType;
                    }
                }
            }
        }

        return colorTypes;
    }
}