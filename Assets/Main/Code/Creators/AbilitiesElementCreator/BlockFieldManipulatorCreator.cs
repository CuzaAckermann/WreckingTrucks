public class BlockFieldManipulatorCreator
{
    public BlockFieldManipulator Create(EventBus eventBus,
        BlockFieldManipulatorSettings blockFieldManipulatorSettings,
        IStateMachine<IApplicationState> stateMachine)
    {
        return new BlockFieldManipulator(eventBus,
            blockFieldManipulatorSettings.AmountShiftedRows,
            stateMachine);
    }
}