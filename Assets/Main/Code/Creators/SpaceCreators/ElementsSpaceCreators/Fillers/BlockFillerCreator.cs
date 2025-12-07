using System;
using System.Collections.Generic;

public class BlockFillerCreator
{
    private readonly FillingStrategiesCreator _fillingStrategiesCreator;
    private readonly BlockFillingCardCreator _fillingCardCreator;
    private readonly RowGeneratorCreator _rowGeneratorCreator;
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly BlockFactory _blockFactory;

    public BlockFillerCreator(FillingStrategiesCreator fillingStrategiesCreator,
                              BlockFillingCardCreator fillingCardCreator,
                              RowGeneratorCreator rowGeneratorCreator,
                              StopwatchCreator stopwatchCreator,
                              BlockFactory blockFactory)
    {
        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
        _fillingCardCreator = fillingCardCreator ?? throw new ArgumentNullException(nameof(fillingCardCreator));
        _rowGeneratorCreator = rowGeneratorCreator ?? throw new ArgumentNullException(nameof(rowGeneratorCreator));
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _blockFactory = blockFactory ?? throw new ArgumentNullException(nameof(blockFactory));
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
                                    fillingStrategy,
                                    _blockFactory,
                                    _rowGeneratorCreator.Create(),
                                    _stopwatchCreator.Create());
    }
}