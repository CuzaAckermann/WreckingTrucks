using System;

public class RecordStorageCreator
{
    private readonly RowGeneratorCreator _rowGeneratorCreator;

    public RecordStorageCreator(RowGeneratorCreator rowGeneratorCreator)
    {
        _rowGeneratorCreator = rowGeneratorCreator ?? throw new ArgumentNullException(nameof(rowGeneratorCreator));
    }

    public IRecordStorage Create(FieldSize fieldSize)
    {
        return _rowGeneratorCreator.Create(fieldSize.AmountLayers, fieldSize.AmountColumns);
    }
}