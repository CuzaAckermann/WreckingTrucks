using System;

public class MoverCreator : ITickableCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly MoverSettings _moverSettings;

    public MoverCreator(ModelProductionCreator modelProductionCreator,
                        MoverSettings moverSettings)
    {
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _moverSettings = moverSettings ?? throw new ArgumentNullException(nameof(moverSettings));
    }

    public event Action<ITickable> StopwatchCreated;

    public Mover Create()
    {
        Mover mover = new Mover(_moverSettings.CapacityMoveables,
                                _modelProductionCreator.CreateModelProduction());

        StopwatchCreated?.Invoke(mover);

        return mover;
    }
}