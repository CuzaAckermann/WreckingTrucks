using System;
using System.Collections.Generic;

public class RecordStorageCreator
{
    private readonly BlockFillingCardCreator _blockFillingCardCreator;
    private readonly RowGeneratorCreator _rowGeneratorCreator;

    private bool _isLayersDefined;

    public RecordStorageCreator(BlockFillingCardCreator blockFillingCardCreator,
                                RowGeneratorCreator rowGeneratorCreator)
    {
        _blockFillingCardCreator = blockFillingCardCreator ?? throw new ArgumentNullException(nameof(blockFillingCardCreator));
        _rowGeneratorCreator = rowGeneratorCreator ?? throw new ArgumentNullException(nameof(rowGeneratorCreator));

        _isLayersDefined = false;
    }

    public void SetBlockLayerSettings(IReadOnlyList<BlockLayerSettings> layers)
    {
        _blockFillingCardCreator.SetBlockLayerSettings(layers);
        _isLayersDefined = true;
    }

    public IRecordStorage Create(FieldSize fieldSize)
    {
        if (_isLayersDefined)
        {
            return _blockFillingCardCreator.Create(fieldSize);
        }
        else
        {
            return _rowGeneratorCreator.CreateBlockFilling(fieldSize.AmountLayers, fieldSize.AmountColumns);
        }
    }
}