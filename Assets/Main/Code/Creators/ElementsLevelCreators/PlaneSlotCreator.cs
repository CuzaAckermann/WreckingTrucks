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
        PositionManipulator positionManipulator = new PositionManipulator();

        PlaneSlot planeSlot = new PlaneSlot(positionManipulator,
                                            new LinearMover(positionManipulator, 10),
                                            new LinearRotator(positionManipulator, 10),
                                            _planeFactory,
                                            planeSpaceSettings.PlaneSlotPosition,
                                            planeSpaceSettings.AmountOfUses);

        eventBus.Invoke(new CreatedSignal<PlaneSlot>(planeSlot));

        return planeSlot;
    }
}