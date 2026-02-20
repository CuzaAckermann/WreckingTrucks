public class ActiveBulletCounter
{
    private readonly EventBus _eventBus;
    private readonly ActiveModelCounter<Bullet> _activeBulletCouner;

    public ActiveBulletCounter(EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);

        _eventBus = eventBus;

        _activeBulletCouner = new ActiveModelCounter<Bullet>(_eventBus);

        _eventBus.Subscribe<CreatedSignal<IDestroyable>>(AddBullet);
        _eventBus.Subscribe<ClearedSignal<Level>>(Finish);
    }

    public int Amount => _activeBulletCouner.Amount;

    private void AddBullet(CreatedSignal<IDestroyable> createdSignal)
    {
        if (Validator.IsRequiredType(createdSignal.Creatable, out Bullet bullet) == false)
        {
            return;
        }

        _eventBus.Invoke(new ActivatedSignal<Bullet>(bullet));
    }

    private void Finish(ClearedSignal<Level> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Level>>(Finish);
        _eventBus.Unsubscribe<CreatedSignal<IDestroyable>>(AddBullet);
    }
}