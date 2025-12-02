using System;

public class RotatorCreator
{
    private readonly TickEngine _tickEngine;
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly RotatorSettings _rotatorSettings;

    public RotatorCreator(TickEngine tickEngine, ModelProductionCreator modelProductionCreator, RotatorSettings rotatorSettings)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _rotatorSettings = rotatorSettings ?? throw new ArgumentNullException(nameof(rotatorSettings));
    }

    public Rotator Create()
    {
        return new Rotator(_tickEngine,
                           _rotatorSettings.CapacityRotatables,
                           _modelProductionCreator.CreateModelProduction());
    }
}