using System;
using System.Collections.Generic;

public class TruckFillerCreator
{
    private readonly FillingStrategiesCreator _fillingStrategiesCreator;
    private readonly TruckGeneratorCreator _truckGeneratorCreator;

    public TruckFillerCreator(FillingStrategiesCreator fillingStrategiesCreator,
                              TruckGeneratorCreator truckGeneratorCreator)
    {
        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
        _truckGeneratorCreator = truckGeneratorCreator ?? throw new ArgumentNullException(nameof(truckGeneratorCreator));
    }

    public TruckFieldFiller Create(Field field,
                                   IReadOnlyList<ColorType> colorTypes)
    {
        TruckGenerator truckGenerator = _truckGeneratorCreator.Create(field, colorTypes);
        FillingStrategy fillingStrategy = _fillingStrategiesCreator.Create(field, truckGenerator);
        fillingStrategy.ActivateNonstopFilling();

        return new TruckFieldFiller(field,
                                    fillingStrategy,
                                    truckGenerator);
    }
}