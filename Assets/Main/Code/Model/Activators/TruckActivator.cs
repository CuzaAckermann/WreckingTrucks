using System;

public class TruckActivator : ModelActivator<Truck>
{
    private readonly TruckField _truckField;
    private readonly Dispencer _cartrigeBoxDispencer;
    private readonly Road _roadForTrucks;
    private readonly BlockTracker _blockTracker;

    public TruckActivator(EventBus eventBus,
                          TruckField truckField,
                          Dispencer cartrigeBoxDispencer,
                          Road roadForTrucks,
                          BlockTracker blockTracker)
                   : base(eventBus)
    {
        _truckField = truckField ?? throw new ArgumentNullException(nameof(truckField));
        _cartrigeBoxDispencer = cartrigeBoxDispencer ?? throw new ArgumentNullException(nameof(cartrigeBoxDispencer));
        _roadForTrucks = roadForTrucks ?? throw new ArgumentNullException(nameof(roadForTrucks));
        _blockTracker = blockTracker ?? throw new ArgumentNullException(nameof(blockTracker));
    }

    protected override void AcrivateModel(SelectedSignal selectedSignal)
    {
        if (selectedSignal.Selectable is not Truck truck)
        {
            return;
        }

        _truckField.TryGetIndexModel(truck, out int _, out int _, out int _);

        if (_truckField.IsFirstInRow(truck) == false)
        {
            return;
        }

        if (_truckField.TryRemoveTruck(truck) == false)
        {
            return;
        }

        EventBus.Invoke(new ActivatedSignal<Truck>(truck));

        if (_cartrigeBoxDispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox) == false)
        {
            return;
        }

        truck.Prepare(_blockTracker, cartrigeBox, _roadForTrucks);
    }
}