using System;

public class EndLevelProcessCreator
{
    private readonly EndLevelRewardCreator _endLevelRewardCreator;

    public EndLevelProcessCreator(EndLevelRewardCreator endLevelRewardCreator)
    {
        _endLevelRewardCreator = endLevelRewardCreator ?? throw new ArgumentNullException(nameof(endLevelRewardCreator));
    }

    public EndLevelProcess Create()
    {
        return new EndLevelProcess(_endLevelRewardCreator.Create());
    }
}