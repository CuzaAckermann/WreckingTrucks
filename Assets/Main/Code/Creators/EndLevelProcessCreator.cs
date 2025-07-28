using System;

public class EndLevelProcessCreator
{
    private readonly EndLevelRewardCreator _endLevelRewardCreator;
    private readonly MoverCreator _moverCreator;
    private readonly ModelFinalizerCreator _modelFinalizerCreator;

    public EndLevelProcessCreator(EndLevelRewardCreator endLevelRewardCreator,
                                  MoverCreator moverCreator,
                                  ModelFinalizerCreator modelFinalizerCreator)
    {
        _endLevelRewardCreator = endLevelRewardCreator ?? throw new ArgumentNullException(nameof(endLevelRewardCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _modelFinalizerCreator = modelFinalizerCreator ?? throw new ArgumentNullException(nameof(modelFinalizerCreator));
    }

    public EndLevelProcess Create()
    {
        EndLevelReward endLevelReward = _endLevelRewardCreator.Create();

        return new EndLevelProcess(endLevelReward,
                                   _moverCreator.Create(endLevelReward, new MoverSettings(100, 35, 0.001f)),
                                   _modelFinalizerCreator.Create());
    }
}