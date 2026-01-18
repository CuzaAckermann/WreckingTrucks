using System;

public class RotatorCreator : ITickableCreator
{
    private readonly RotatorSettings _rotatorSettings;

    public RotatorCreator(RotatorSettings rotatorSettings)
    {
        _rotatorSettings = rotatorSettings ?? throw new ArgumentNullException(nameof(rotatorSettings));
    }

    public event Action<ITickable> TickableCreated;

    public Rotator Create(EventBus eventBus)
    {
        Rotator rotator = new Rotator(eventBus, _rotatorSettings.CapacityRotatables);

        TickableCreated?.Invoke(rotator);

        return rotator;
    }
}