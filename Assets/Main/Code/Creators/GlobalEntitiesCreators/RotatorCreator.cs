using System;

public class RotatorCreator : ITickableCreator
{
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly RotatorSettings _rotatorSettings;

    public RotatorCreator(ModelProductionCreator modelProductionCreator, RotatorSettings rotatorSettings)
    {
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _rotatorSettings = rotatorSettings ?? throw new ArgumentNullException(nameof(rotatorSettings));
    }

    public event Action<ITickable> StopwatchCreated;

    public Rotator Create()
    {
        Rotator rotator = new Rotator(_rotatorSettings.CapacityRotatables,
                                      _modelProductionCreator.CreateModelProduction());

        StopwatchCreated?.Invoke(rotator);

        return rotator;
    }
}