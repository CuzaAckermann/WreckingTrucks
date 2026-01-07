using System;

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

    public void SetBlockFieldSettings(BlockFieldSettings blockFieldSettings)
    {
        _blockFillingCardCreator.SetBlockFieldSettings(blockFieldSettings);
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
            return _rowGeneratorCreator.Create(fieldSize.AmountLayers, fieldSize.AmountColumns);
        }
    }
}