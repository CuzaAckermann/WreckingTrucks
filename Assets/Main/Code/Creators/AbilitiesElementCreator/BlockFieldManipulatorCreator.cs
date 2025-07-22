public class BlockFieldManipulatorCreator
{
    public BlockFieldManipulator Create(BlockFieldManipulatorSettings blockFieldManipulatorSettings)
    {
        return new BlockFieldManipulator(blockFieldManipulatorSettings.AmountShiftedRows);
    }
}