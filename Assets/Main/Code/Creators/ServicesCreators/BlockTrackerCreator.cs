public class BlockTrackerCreator
{
    private readonly BlockTracker _blockTracker;

    public BlockTrackerCreator(BlockFieldCreator blockFieldCreator)
    {
        _blockTracker = new BlockTracker(blockFieldCreator);
    }

    public BlockTracker Create()
    {
        return _blockTracker;
    }
}