using System;

public class RotatorUpdaterCreator : ITickableCreator
{
    private readonly RotatorUpdaterSettings _rotatorSettings;

    public RotatorUpdaterCreator(RotatorUpdaterSettings rotatorSettings)
    {
        _rotatorSettings = rotatorSettings ?? throw new ArgumentNullException(nameof(rotatorSettings));
    }

    public event Action<ITickable> TickableCreated;

    public RotatorUpdater Create(EventBus eventBus)
    {
        RotatorUpdater rotator = new RotatorUpdater(eventBus, _rotatorSettings.CapacityRotatables);

        TickableCreated?.Invoke(rotator);

        return rotator;
    }
}