using System;

public class PlaneSlotCreator
{
    private readonly ModelFactory<Plane> _planeFactory;

    public PlaneSlotCreator(ModelFactory<Plane> planeFactory)
    {
        _planeFactory = planeFactory ?? throw new ArgumentNullException(nameof(planeFactory));
    }

    public PlaneSlot Create(PlaneSpaceSettings planeSpaceSettings)
    {
        PlaneSlot planeSlot = new PlaneSlot(10,
                                            10,
                                            _planeFactory,
                                            planeSpaceSettings.PlaneSlotPosition,
                                            planeSpaceSettings.AmountOfUses);

        return planeSlot;
    }
}