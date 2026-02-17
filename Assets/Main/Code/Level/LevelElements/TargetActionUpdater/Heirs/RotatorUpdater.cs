public class RotatorUpdater : TargetActionUpdater<IRotator>
{
    public RotatorUpdater(EventBus eventBus, int capacity)
                   : base(eventBus, capacity)
    {

    }

    protected override IRotator GetTargetAction(Model model)
    {
        return model.Rotator;
    }
}