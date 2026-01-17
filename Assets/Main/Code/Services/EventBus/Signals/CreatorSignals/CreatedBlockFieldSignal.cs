using System;

public class CreatedBlockFieldSignal
{
    private readonly BlockField _blockField;

    public CreatedBlockFieldSignal(BlockField blockField)
    {
        _blockField = blockField ?? throw new ArgumentNullException(nameof(blockField));
    }

    public BlockField BlockField => _blockField;
}