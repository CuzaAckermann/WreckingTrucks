using System;

public class MoverCreator
{
    private readonly TickEngine _tickEngine;
    private readonly ModelProductionCreator _modelProductionCreator;
    private readonly MoverSettings _moverSettings;

    public MoverCreator(TickEngine tickEngine, ModelProductionCreator modelProductionCreator, MoverSettings moverSettings)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
        _modelProductionCreator = modelProductionCreator ?? throw new ArgumentNullException(nameof(modelProductionCreator));
        _moverSettings = moverSettings ?? throw new ArgumentNullException(nameof(moverSettings));
    }

    public Mover Create()
    {
        return new Mover(_tickEngine,
                         _moverSettings.CapacityMoveables,
                         _modelProductionCreator.CreateModelProduction());
    }
}