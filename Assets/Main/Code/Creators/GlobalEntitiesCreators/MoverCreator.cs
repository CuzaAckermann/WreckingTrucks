using System;

public class MoverCreator : ITickableCreator
{
    private readonly MoverUpdaterSettings _moverSettings;

    public MoverCreator(MoverUpdaterSettings moverSettings)
    {
        _moverSettings = moverSettings ?? throw new ArgumentNullException(nameof(moverSettings));
    }

    public event Action<ITickable> TickableCreated;

    public MoverUpdater Create(EventBus eventBus)
    {
        MoverUpdater mover = new MoverUpdater(eventBus, _moverSettings.CapacityMoveables);

        TickableCreated?.Invoke(mover);

        return mover;
    }
}