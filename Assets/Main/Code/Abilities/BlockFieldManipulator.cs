public class BlockFieldManipulator : IApplicationAbility
{
    private readonly EventBus _eventBus;

    private readonly int _amountShiftedRows;

    private Field _blockField;

    public BlockFieldManipulator(EventBus eventBus, int amountShiftedRows)
    {
        Validator.ValidateNotNull(eventBus);
        Validator.ValidateMin(amountShiftedRows, 0, true);

        _eventBus = eventBus;
        _amountShiftedRows = amountShiftedRows;
    }

    public void Start()
    {
        _eventBus.Subscribe<CreatedSignal<BlockField>>(SetField);
    }

    public void Finish()
    {
        _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetField);
    }

    public void OpenField()
    {
        _blockField.ShowRows(_amountShiftedRows);
    }

    public void CloseField()
    {
        _blockField.HideRows();
    }

    private void SetField(CreatedSignal<BlockField> blockFieldCreatedSignal)
    {
        _blockField = blockFieldCreatedSignal.Creatable;
    }
}