public class TargetActionLockedStorage<T> : LockedStorage<T> where T : ITargetAction
{
    public TargetActionLockedStorage(int capacity) : base(capacity)
    {

    }

    protected override void SubscribeToCreated(T item)
    {
        base.SubscribeToCreated(item);

        item.TargetChanged += OnTargetChanged;
    }

    protected override void UnsubscribeFromCreated(T item)
    {
        base.UnsubscribeFromCreated(item);

        item.TargetChanged -= OnTargetChanged;
    }

    protected override void SubscribeToActive(T item)
    {
        item.TargetReached += OnTargetReached;
    }

    protected override void UnsubscribeFromActive(T item)
    {
        item.TargetReached -= OnTargetReached;
    }

    private void OnTargetChanged(ITargetAction item)
    {
        if (Validator.IsRequiredType(item, out T targetAction) == false)
        {
            return;
        }

        Add(targetAction);
    }

    private void OnTargetReached(ITargetAction item)
    {
        if (Validator.IsRequiredType(item, out T targetAction) == false)
        {
            return;
        }

        Remove(targetAction);
    }
}
