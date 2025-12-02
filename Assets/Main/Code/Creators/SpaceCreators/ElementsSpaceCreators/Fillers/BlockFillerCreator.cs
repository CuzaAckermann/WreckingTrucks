using System;
using System.Collections.Generic;

public class BlockFillerCreator
{
    private readonly FillingStrategiesCreator _fillingStrategiesCreator;
    private readonly BlockFillingCardCreator _fillingCardCreator;

    public BlockFillerCreator(FillingStrategiesCreator fillingStrategiesCreator,
                              BlockFillingCardCreator fillingCardCreator)
    {
        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
        _fillingCardCreator = fillingCardCreator ?? throw new ArgumentNullException(nameof(fillingCardCreator));
    }

    public void SetBlockLayerSettings(IReadOnlyList<BlockLayerSettings> blockLayerSettings)
    {
        _fillingCardCreator.SetBlockLayerSettings(blockLayerSettings);
    }

    public BlockFieldFiller Create(Field field, BlockSpaceSettings blockSpaceSettings)
    {
        FillingStrategy fillingStrategy = _fillingStrategiesCreator.Create(blockSpaceSettings.FillerSettings);
        fillingStrategy.PrepareFilling(field, _fillingCardCreator.Create(blockSpaceSettings.FieldSettings.FieldSize));

        return new BlockFieldFiller(field,
                                    fillingStrategy);
    }
}