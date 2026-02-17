using System;

public class PlaneActivator : ModelActivator<Plane>
{
    private readonly ModelSlot<Plane> _planeSlot;
    private readonly Dispencer _cartrigeBoxDispencer;
    private readonly BlockField _blockField;
    private readonly Road _roadForPlane;

    public PlaneActivator(EventBus eventBus,
                          BlockField blocksField,
                          Dispencer cartrigeBoxDispencer,
                          Road roadForPlane,
                          ModelSlot<Plane> planeSlot)
                   : base(eventBus)
    {
        _planeSlot = planeSlot ?? throw new ArgumentNullException(nameof(planeSlot));
        _cartrigeBoxDispencer = cartrigeBoxDispencer ?? throw new ArgumentNullException(nameof(cartrigeBoxDispencer));
        _blockField = blocksField ?? throw new ArgumentNullException(nameof(blocksField));
        _roadForPlane = roadForPlane ?? throw new ArgumentNullException(nameof(roadForPlane));
    }

    protected override void AcrivateModel(SelectedSignal<Model> planeSelectedSignal)
    {
        if (planeSelectedSignal.Selectable is not Plane plane)
        {
            return;
        }

        if (_planeSlot.TryGetModel(out Plane planeInSlot) == false)
        {
            return;
        }

        if (planeInSlot != plane)
        {
            return;
        }

        if (planeInSlot.IsWork)
        {
            return;
        }

        EventBus.Invoke(new ActivatedSignal<Plane>(plane));

        if (_cartrigeBoxDispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox) == false)
        {
            return;
        }

        plane.Prepare(_blockField, cartrigeBox, _roadForPlane);
    }
}