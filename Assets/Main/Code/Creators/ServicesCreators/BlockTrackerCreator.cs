public class BlockTrackerCreator
{
    private readonly BlockTracker _blockTracker;

    public BlockTrackerCreator(EventBus eventBus)
    {
        _blockTracker = new BlockTracker(eventBus);
    }

    public BlockTracker Create()
    {
        return _blockTracker;
    }
}