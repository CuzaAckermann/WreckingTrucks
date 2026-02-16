using System;

public class RotatorUpdaterCreator : ITickableCreator
{
    private readonly UpdaterSettings _rotatorSettings;

    public RotatorUpdaterCreator(UpdaterSettings rotatorSettings)
    {
        Validator.ValidateNotNull(rotatorSettings);

        _rotatorSettings = rotatorSettings;
    }

    public event Action<ITickable> TickableCreated;

    public RotatorUpdater Create(EventBus eventBus)
    {
        RotatorUpdater rotator = new RotatorUpdater(eventBus, _rotatorSettings.Capacity);

        TickableCreated?.Invoke(rotator);

        return rotator;
    }
}