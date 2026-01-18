using System;

public class MoverCreator : ITickableCreator
{
    private readonly MoverSettings _moverSettings;

    public MoverCreator(MoverSettings moverSettings)
    {
        _moverSettings = moverSettings ?? throw new ArgumentNullException(nameof(moverSettings));
    }

    public event Action<ITickable> TickableCreated;

    public Mover Create(EventBus eventBus)
    {
        Mover mover = new Mover(eventBus, _moverSettings.CapacityMoveables);

        TickableCreated?.Invoke(mover);

        return mover;
    }
}