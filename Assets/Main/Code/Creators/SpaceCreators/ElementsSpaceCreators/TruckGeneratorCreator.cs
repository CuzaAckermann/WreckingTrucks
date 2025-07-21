using System;
using System.Collections.Generic;

public class TruckGeneratorCreator
{
    private readonly CreatedTypesCreator<TruckTypeConverter> _truckTypesCreator;

    public TruckGeneratorCreator(CreatedTypesCreator<TruckTypeConverter> truckTypesCreator)
    {
        _truckTypesCreator = truckTypesCreator ?? throw new ArgumentNullException(nameof(truckTypesCreator));
    }

    public ModelTypeGenerator<Truck> Create(IReadOnlyList<ColorType> colorTypes, float amountProbabilityReduction)
    {
        List<Type> truckTypes = _truckTypesCreator.Create(colorTypes);
        ModelProbabilitySettings<Truck> modelProbabilitySettings = new ModelProbabilitySettings<Truck>(truckTypes);
        ModelTypeGenerator<Truck> truckGenerator = new ModelTypeGenerator<Truck>(modelProbabilitySettings,
                                                                                 amountProbabilityReduction);

        return truckGenerator;
    }
}