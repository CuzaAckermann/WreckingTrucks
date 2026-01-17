using System;

public class PlaneSlotCreator
{
    private readonly ModelFactory<Plane> _planeFactory;

    public PlaneSlotCreator(ModelFactory<Plane> planeFactory)
    {
        _planeFactory = planeFactory ?? throw new ArgumentNullException(nameof(planeFactory));
    }

    public PlaneSlot Create(PlaneSpaceSettings planeSpaceSettings, EventBus eventBus)
    {
        PlaneSlot planeSlot = new PlaneSlot(10,
                                            10,
                                            _planeFactory,
                                            planeSpaceSettings.PlaneSlotPosition,
                                            planeSpaceSettings.AmountOfUses);

        eventBus.Invoke(new CreatedPlaneSlotSignal(planeSlot));

        return planeSlot;
    }
}