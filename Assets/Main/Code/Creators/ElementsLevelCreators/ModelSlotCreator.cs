public class ModelSlotCreator
{
    public ModelSlot<M> Create<M>(PlaneSpaceSettings planeSpaceSettings, EventBus eventBus) where M : Model
    {
        PositionManipulator positionManipulator = new PositionManipulator();

        ModelSlot<M> modelSlot = new ModelSlot<M>(positionManipulator,
                                                  new LinearMover(positionManipulator, 10),
                                                  new LinearRotator(positionManipulator, 10),
                                                  planeSpaceSettings.PlaneSlotPosition,
                                                  planeSpaceSettings.AmountOfUses);

        eventBus.Invoke(new CreatedSignal<ModelSlot<M>>(modelSlot));

        return modelSlot;
    }
}