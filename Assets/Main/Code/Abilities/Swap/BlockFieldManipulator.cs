using System;

public class BlockFieldManipulator
{
    private readonly EventBus _eventBus;

    private readonly int _amountShiftedRows;

    private Field _blockField;

    public BlockFieldManipulator(EventBus eventBus, int amountShiftedRows)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _amountShiftedRows = amountShiftedRows > 0 ? amountShiftedRows : throw new ArgumentOutOfRangeException(nameof(amountShiftedRows));

        _eventBus.Subscribe<ClearedSignal<Game>>(Clear);

        _eventBus.Subscribe<CreatedSignal<BlockField>>(SetField);
    }

    public void OpenField()
    {
        _blockField.ShowRows(_amountShiftedRows);
    }

    public void CloseField()
    {
        _blockField.HideRows();
    }

    private void Clear(ClearedSignal<Game> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Game>>(Clear);

        _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetField);
    }

    private void SetField(CreatedSignal<BlockField> blockFieldCreatedSignal)
    {
        _blockField = blockFieldCreatedSignal.Creatable;
    }
}