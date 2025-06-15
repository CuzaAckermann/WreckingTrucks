using System;

public class LevelSettings
{
    private readonly SpaceSettings _blocksSpaceSettings;
    private readonly SpaceSettings _trucksSpaceSettings;

    public LevelSettings(SpaceSettings blocksSpaceSettings, SpaceSettings trucksSpaceSettings)
    {
        _blocksSpaceSettings = blocksSpaceSettings ?? throw new ArgumentNullException(nameof(blocksSpaceSettings));
        _trucksSpaceSettings = trucksSpaceSettings ?? throw new ArgumentNullException(nameof(trucksSpaceSettings));
    }

    public SpaceSettings BlocksSpaceSettings => _blocksSpaceSettings;

    public SpaceSettings TrucksSpaceSettings => _trucksSpaceSettings;
}