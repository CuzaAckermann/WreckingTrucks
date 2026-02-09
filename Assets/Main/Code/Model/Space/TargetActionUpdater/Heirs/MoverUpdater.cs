public class MoverUpdater : TargetActionUpdater<IMover>
{
    public MoverUpdater(EventBus eventBus, int capacity)
                 : base(eventBus, capacity)
    {

    }

    protected override IMover GetTargetAction(Model model)
    {
        return model.Mover;
    }
}