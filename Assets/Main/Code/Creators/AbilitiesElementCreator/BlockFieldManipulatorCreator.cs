public class BlockFieldManipulatorCreator
{
    private readonly BlockFieldManipulatorSettings _blockFieldManipulatorSettings;
    private readonly EventBus _eventBus;

    public BlockFieldManipulatorCreator(BlockFieldManipulatorSettings blockFieldManipulatorSettings, EventBus eventBus)
    {
        Validator.ValidateNotNull(blockFieldManipulatorSettings, eventBus);

        _blockFieldManipulatorSettings = blockFieldManipulatorSettings;
        _eventBus = eventBus;
    }

    public BlockFieldManipulator Create()
    {
        return new BlockFieldManipulator(_eventBus,
                                         _blockFieldManipulatorSettings.AmountShiftedRows);
    }
}