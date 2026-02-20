public class ModelSlotCreator
{
    public ModelSlot Create(PlaneSpaceSettings planeSpaceSettings, EventBus eventBus)
    {
        Placeable positionManipulator = new Placeable();

        ModelSlot modelSlot = new ModelSlot(positionManipulator,
                                                  new LinearMover(positionManipulator, 10),
                                                  new LinearRotator(positionManipulator, 10),
                                                  planeSpaceSettings.PlaneSlotPosition,
                                                  new Placer(eventBus),
                                                  planeSpaceSettings.AmountOfUses);

        eventBus.Invoke(new CreatedSignal<ModelSlot>(modelSlot));

        return modelSlot;
    }
}