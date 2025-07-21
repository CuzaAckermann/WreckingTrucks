using System;
using UnityEngine;

public class TruckSpaceCreator
{
    private readonly TruckFieldCreator _fieldCreator;
    private readonly MoverCreator _moverCreator;
    private readonly FillerCreator _fillerCreator;
    private readonly ModelFinalizerCreator _modelFinalizerCreator;
    private readonly ModelProductionCreator _productionCreator;
    private readonly TruckGeneratorCreator _truckGeneratorCreator;

    public TruckSpaceCreator(TruckFieldCreator fieldCreator,
                             MoverCreator moverCreator,
                             FillerCreator fillerCreator,
                             ModelFinalizerCreator modelFinalizerCreator,
                             ModelProductionCreator productionCreator,
                             TruckGeneratorCreator truckGeneratorCreator)
    {
        _fieldCreator = fieldCreator ?? throw new ArgumentNullException(nameof(fieldCreator));
        _moverCreator = moverCreator ?? throw new ArgumentNullException(nameof(moverCreator));
        _fillerCreator = fillerCreator ?? throw new ArgumentNullException(nameof(fillerCreator));
        _modelFinalizerCreator = modelFinalizerCreator ?? throw new ArgumentNullException(nameof(modelFinalizerCreator));
        _productionCreator = productionCreator ?? throw new ArgumentNullException(nameof(productionCreator));
        _truckGeneratorCreator = truckGeneratorCreator ?? throw new ArgumentNullException(nameof(truckGeneratorCreator));
    }

    public TruckSpace Create(Transform fieldTransform, TruckSpaceSettings truckSpaceSettings)
    {
        TruckField truckField = _fieldCreator.Create(fieldTransform, truckSpaceSettings.FieldSettings.FieldSize);

        return new TruckSpace(truckField,
                              _moverCreator.Create(truckField, truckSpaceSettings.MoverSettings),
                              _fillerCreator.Create(truckField),
                              _modelFinalizerCreator.Create(),
                              _productionCreator.CreateTruckProduction(),
                              _truckGeneratorCreator.Create(truckSpaceSettings.FieldSettings.Types,
                                                            truckSpaceSettings.FieldSettings.AmountProbabilityReduction));
    }
}