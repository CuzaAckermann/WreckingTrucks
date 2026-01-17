using System;

public class CreatedPlaneSlotSignal
{
    private readonly PlaneSlot _planeSlot;

    public CreatedPlaneSlotSignal(PlaneSlot planeSlot)
    {
        _planeSlot = planeSlot ?? throw new ArgumentNullException(nameof(planeSlot));
    }

    public PlaneSlot PlaneSlot => _planeSlot;
}