public class TickableLockedStorage : LockedStorage<ITickable>
{
    public TickableLockedStorage(int capacity) : base(capacity)
    {

    }

    protected override void SubscribeToCreated(ITickable item)
    {
        base.SubscribeToCreated(item);

        item.Activated += Add;
    }

    protected override void UnsubscribeFromCreated(ITickable item)
    {
        base.UnsubscribeFromCreated(item);

        item.Activated -= Add;
    }

    protected override void SubscribeToActive(ITickable item)
    {
        item.Deactivated += Remove;
    }

    protected override void UnsubscribeFromActive(ITickable item)
    {
        item.Deactivated -= Remove;
    }
}