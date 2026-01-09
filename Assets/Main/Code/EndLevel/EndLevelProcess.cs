using System;

public class EndLevelProcess
{
    private readonly EndLevelReward _endLevelReward;

    public EndLevelProcess(EndLevelReward endLevelReward)
    {
        _endLevelReward = endLevelReward ?? throw new ArgumentNullException(nameof(endLevelReward));
    }

    public void Clear()
    {

    }

    public void Enable()
    {

    }

    public void Disable()
    {

    }

    public void SetDispencer(Dispencer dispencer)
    {
        _endLevelReward.StartCollectingCartrigeBoxes(dispencer);
    }
}