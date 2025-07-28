using System;

public class EndLevelProcess
{
    private readonly EndLevelReward _endLevelReward;
    private readonly Mover _mover;
    private readonly ModelFinalizer _modelFinalizer;

    public EndLevelProcess(EndLevelReward endLevelReward,
                           Mover mover,
                           ModelFinalizer modelFinalizer)
    {
        _endLevelReward = endLevelReward ?? throw new ArgumentNullException(nameof(endLevelReward));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _modelFinalizer = modelFinalizer ?? throw new ArgumentNullException(nameof(modelFinalizer));
    }

    public void Clear()
    {
        _mover.Clear();

        _modelFinalizer.Disable();
        _modelFinalizer.Clear();
    }

    public void Enable()
    {
        _modelFinalizer.AddNotifier(_endLevelReward);

        _mover.Enable();
        _modelFinalizer.Enable();
    }

    public void Disable()
    {
        _mover.Disable();
    }

    public void SetCartrigeBoxSpace(CartrigeBoxSpace cartrigeBoxSpace)
    {
        _endLevelReward.TakeCartrigeBoxes(cartrigeBoxSpace);
    }
}