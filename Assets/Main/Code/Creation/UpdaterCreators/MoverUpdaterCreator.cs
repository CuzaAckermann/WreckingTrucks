using System;

public class MoverUpdaterCreator : ITickableCreator
{
    private readonly UpdaterSettings _moverSettings;

    public MoverUpdaterCreator(UpdaterSettings moverSettings)
    {
        _moverSettings = moverSettings ?? throw new ArgumentNullException(nameof(moverSettings));
    }

    public event Action<ITickable> TickableCreated;

    public MoverUpdater Create(EventBus eventBus)
    {
        MoverUpdater mover = new MoverUpdater(eventBus, _moverSettings.Capacity);

        TickableCreated?.Invoke(mover);

        return mover;
    }
}