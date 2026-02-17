public class JellyLockedStorage : LockedStorage<Jelly>
{
    public JellyLockedStorage(int capacity) : base(capacity)
    {

    }

    protected override void SubscribeToCreated(Jelly item)
    {
        item.Started += Add;

        base.SubscribeToCreated(item);
    }

    protected override void UnsubscribeFromCreated(Jelly item)
    {
        item.Started -= Add;

        base.UnsubscribeFromCreated(item);
    }

    protected override void SubscribeToActive(Jelly item)
    {
        item.Finished += Remove;
    }

    protected override void UnsubscribeFromActive(Jelly item)
    {
        item.Finished -= Remove;
    }
}