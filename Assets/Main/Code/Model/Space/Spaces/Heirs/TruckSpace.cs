using System;

public class TruckSpace : Space<TruckField>
{
    private readonly ModelProduction<Truck> _truckProduction;
    private readonly ModelTypeGenerator<Truck> _generator;

    public TruckSpace(TruckField truckField,
                      Mover mover,
                      Filler filler,
                      ModelProduction<Truck> truckProduction,
                      ModelTypeGenerator<Truck> generator)
               : base(truckField,
                      mover,
                      filler)
    {
        _truckProduction = truckProduction ?? throw new ArgumentNullException(nameof(truckProduction));
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
    }

    public bool IsFirstInRow(Model model)
    {
        if (Field.TryGetIndexModel(model, out int _, out int _, out int indexOfRow))
        {
            if (indexOfRow == 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool TryRemoveTruck(Truck truck)
    {
        return Field.TryRemoveTruck(truck);
    }

    public override void Enable()
    {
        Field.TruckRemoved += OnTruckRemoved;

        base.Enable();
    }

    public override void Disable()
    {
        Field.TruckRemoved -= OnTruckRemoved;

        base.Disable();
    }

    private void OnTruckRemoved(int indexOfLayer, int indexOfColumn)
    {
        Truck truck = _truckProduction.CreateModel(_generator.Generate());
        Filler.PlaceModel(truck, indexOfLayer, indexOfColumn);
    }
}