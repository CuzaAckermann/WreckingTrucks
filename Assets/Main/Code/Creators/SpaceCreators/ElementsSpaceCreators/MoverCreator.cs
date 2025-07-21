using System;

public class MoverCreator
{
    private readonly TickEngine _tickEngine;

    public MoverCreator(TickEngine tickEngine)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public Mover Create(IModelPositionObserver positionObserver, MoverSettings moverSettings)
    {
        if (moverSettings == null)
        {
            throw new ArgumentNullException(nameof(moverSettings));
        }

        return new Mover(_tickEngine,
                         positionObserver,
                         moverSettings.CapacityMoveables,
                         moverSettings.MovementSpeed,
                         moverSettings.MinSqrDistanceToTargetPosition);
    }
}