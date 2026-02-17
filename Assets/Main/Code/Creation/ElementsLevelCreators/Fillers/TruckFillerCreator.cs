using System;
using System.Collections.Generic;

public class TruckFillerCreator
{
    private readonly FillingStrategiesCreator _fillingStrategiesCreator;
    private readonly ModelColorGeneratorCreator _truckGeneratorCreator;

    public TruckFillerCreator(FillingStrategiesCreator fillingStrategiesCreator,
                              ModelColorGeneratorCreator truckGeneratorCreator)
    {
        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
        _truckGeneratorCreator = truckGeneratorCreator ?? throw new ArgumentNullException(nameof(truckGeneratorCreator));
    }

    public TruckFieldFiller Create(Field field,
                                   IReadOnlyList<ColorType> colorTypes,
                                   EventBus eventBus)
    {
        ModelColorGenerator truckGenerator = _truckGeneratorCreator.Create(field, colorTypes);
        FillingStrategy<Truck> fillingStrategy = _fillingStrategiesCreator.Create<Truck>(field, truckGenerator);
        fillingStrategy.ActivateNonstopFilling();

        return new TruckFieldFiller(field,
                                    fillingStrategy,
                                    truckGenerator,
                                    eventBus);
    }
}