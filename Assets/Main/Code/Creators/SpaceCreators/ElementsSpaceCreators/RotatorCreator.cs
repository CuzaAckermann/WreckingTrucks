using System;

public class RotatorCreator
{
    private readonly TickEngine _tickEngine;

    public RotatorCreator(TickEngine tickEngine)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public Rotator Create(IModelPositionObserver notifier, RotatorSettings rotatorSettings)
    {
        if (rotatorSettings == null)
        {
            throw new ArgumentNullException(nameof(rotatorSettings));
        }

        return new Rotator(_tickEngine,
                           notifier,
                           rotatorSettings.CapacityRotatables,
                           rotatorSettings.RotationSpeed,
                           rotatorSettings.MinAngleToFinish);
    }
}