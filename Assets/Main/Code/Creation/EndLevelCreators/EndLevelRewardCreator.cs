using System;

public class EndLevelRewardCreator
{
    private readonly EventBus _eventBus;
    private readonly UIPositionDeterminator _uIPositionDeterminator;

    public EndLevelRewardCreator(EventBus eventBus, UIPositionDeterminator uIPositionDeterminator)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _uIPositionDeterminator = uIPositionDeterminator ? uIPositionDeterminator : throw new ArgumentNullException(nameof(uIPositionDeterminator));
    }

    public EndLevelReward Create()
    {
        EndLevelReward endLevelReward = new EndLevelReward(_uIPositionDeterminator, 0.01f);

        _eventBus.Invoke(new CreatedSignal<ICommandCreator>(endLevelReward));

        return endLevelReward;
    }
}