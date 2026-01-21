public class BlockFieldManipulatorCreator
{
    public BlockFieldManipulator Create(EventBus eventBus, BlockFieldManipulatorSettings blockFieldManipulatorSettings)
    {
        return new BlockFieldManipulator(eventBus, blockFieldManipulatorSettings.AmountShiftedRows);
    }
}