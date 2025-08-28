using System;
using UnityEngine;

public class TruckSpaceCreator
{
    private readonly TruckFieldCreator _fieldCreator;
    private readonly MoverCreator _moverCreator;
    private readonly FillerCreator _fillerCreator;
    private readonly ModelProductionCreator _productionCreator;
    private readonly TruckGeneratorCreator _truckGeneratorCreator;
    private readonly TruckFillingCardCreator _truckFillingCardCreator;

    public TruckSpaceCreator(TruckFieldCreator fieldCreator,
                             MoverCreator moverCreator,
                             FillerCreator fillerCreator,
                             ModelProductionCreator productionCreator,
                             TruckGeneratorCreator truckGeneratorCreator,
                             TruckFillingCardCreator truckFillingCardCreator)
    {
        _fieldCreator = fieldCreator ?? throw new ArgumentNullException(nameof(fieldCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _fillerCreator = fillerCreator ?? throw new ArgumentNullException(nameof(fillerCreator));
        _productionCreator = productionCreator ?? throw new ArgumentNullException(nameof(productionCreator));
        _truckGeneratorCreator = truckGeneratorCreator ?? throw new ArgumentNullException(nameof(truckGeneratorCreator));
        _truckFillingCardCreator = truckFillingCardCreator ?? throw new ArgumentNullException(nameof(truckFillingCardCreator));
    }

    public TruckSpace Create(Transform fieldTransform, TruckSpaceSettings truckSpaceSettings)
    {
        TruckField truckField = _fieldCreator.Create(fieldTransform,
                                                     truckSpaceSettings.FieldSettings.FieldSize,
                                                     truckSpaceSettings.FieldIntervals);

        ModelTypeGenerator<Truck> truckTypeGenerator = _truckGeneratorCreator.Create(truckSpaceSettings.FieldSettings.Types,
                                                                                     truckSpaceSettings.TruckTypeGeneratorSettings);

        _truckFillingCardCreator.SetTruckTypeGenerator(truckTypeGenerator);

        return new TruckSpace(truckField,
                              _moverCreator.Create(truckField, truckSpaceSettings.MoverSettings),
                              _fillerCreator.Create(truckSpaceSettings.FillerSettings,
                                                    truckField,
                                                    _truckFillingCardCreator.Create(truckSpaceSettings.FieldSettings)),
                              _productionCreator.CreateTruckProduction(),
                              truckTypeGenerator);
    }
}