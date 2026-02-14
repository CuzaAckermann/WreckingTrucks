public class BlockFieldManipulator
{
    private readonly EventBus _eventBus;

    private readonly int _amountShiftedRows;

    private readonly IStateMachine<IApplicationState> _stateMachine;

    private Field _blockField;

    public BlockFieldManipulator(EventBus eventBus, int amountShiftedRows, IStateMachine<IApplicationState> stateMachine)
    {
        Validator.ValidateNotNull(eventBus, stateMachine);
        Validator.ValidateMin(amountShiftedRows, 0, true);

        _eventBus = eventBus;
        _stateMachine = stateMachine;
        _amountShiftedRows = amountShiftedRows;

        _stateMachine.StateChanged += Clear;
        Clear(_stateMachine.CurrentState);

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

    private void Clear(IApplicationState applicationState)
    {
        if (applicationState is not OnDestroyApplicationState)
        {
            return;
        }

        _stateMachine.StateChanged -= Clear;

        _eventBus.Unsubscribe<CreatedSignal<BlockField>>(SetField);
    }

    private void SetField(CreatedSignal<BlockField> blockFieldCreatedSignal)
    {
        _blockField = blockFieldCreatedSignal.Creatable;
    }
}