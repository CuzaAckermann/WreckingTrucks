using System;

public class EndLevelRewardCreator
{
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly UIPositionDeterminator _uIPositionDeterminator;

    public EndLevelRewardCreator(StopwatchCreator stopwatchCreator,
                                 UIPositionDeterminator uIPositionDeterminator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _uIPositionDeterminator = uIPositionDeterminator ? uIPositionDeterminator : throw new ArgumentNullException(nameof(uIPositionDeterminator));
    }

    public EndLevelReward Create()
    {
        return new EndLevelReward(_uIPositionDeterminator, _stopwatchCreator.Create());
    }
}