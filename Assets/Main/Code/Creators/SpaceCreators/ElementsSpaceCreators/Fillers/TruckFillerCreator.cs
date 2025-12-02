using System;
using System.Collections.Generic;

public class TruckFillerCreator
{
    private readonly FillingStrategiesCreator _fillingStrategiesCreator;
    private readonly TruckFillingCardCreator _fillingCardCreator;
    private readonly TruckGeneratorCreator _truckGeneratorCreator;

    public TruckFillerCreator(FillingStrategiesCreator fillingStrategiesCreator,
                              TruckFillingCardCreator fillingCardCreator,
                              TruckGeneratorCreator truckGeneratorCreator)
    {
        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
        _fillingCardCreator = fillingCardCreator ?? throw new ArgumentNullException(nameof(fillingCardCreator));
        _truckGeneratorCreator = truckGeneratorCreator ?? throw new ArgumentNullException(nameof(truckGeneratorCreator));
    }

    public void Prepare(FieldSize fieldSize, IReadOnlyList<ColorType> colorType)
    {
        _fillingCardCreator.SetFieldSize(fieldSize);
        _fillingCardCreator.SetColorTypes(colorType);
    }

    public TruckFieldFiller Create(Field field, TruckSpaceSettings truckSpaceSettings, IReadOnlyList<ColorType> colorTypes)
    {
        FillingStrategy fillingStrategy = _fillingStrategiesCreator.Create(truckSpaceSettings.FillerSettings);
        fillingStrategy.PrepareFilling(field, _fillingCardCreator.Create(truckSpaceSettings.FieldSettings.FieldSize));
        TruckGenerator truckGenerator = _truckGeneratorCreator.Create();
        truckGenerator.SetColorTypes(colorTypes);

        return new TruckFieldFiller(field,
                                    fillingStrategy,
                                    truckGenerator);
    }
}