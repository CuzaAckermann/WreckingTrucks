using System;

public class BlockTrackerCreator
{
    private readonly BlockTrackerSettings _blockTrackerSettings;

    public BlockTrackerCreator(BlockTrackerSettings blockTrackerSettings)
    {
        _blockTrackerSettings = blockTrackerSettings ?? throw new ArgumentNullException(nameof(blockTrackerSettings));
    }

    public BlockTracker Create()
    {
        return new BlockTracker(_blockTrackerSettings.Range);
    }
}