using System;

public class EndLevelRewardCreator
{
    private readonly StopwatchCreator _stopwatchCreator;
    private readonly UIPositionDeterminatorCreator _uIPositionDeterminatorCreator;

    public EndLevelRewardCreator(StopwatchCreator stopwatchCreator,
                                 UIPositionDeterminatorCreator uIPositionDeterminatorCreator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _uIPositionDeterminatorCreator = uIPositionDeterminatorCreator ?? throw new ArgumentNullException(nameof(uIPositionDeterminatorCreator));
    }

    public EndLevelReward Create()
    {
        return new EndLevelReward(_uIPositionDeterminatorCreator.Create(), _stopwatchCreator.Create());
    }
}