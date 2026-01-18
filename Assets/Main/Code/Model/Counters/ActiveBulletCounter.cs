using System;

public class ActiveBulletCounter
{
    private readonly EventBus _eventBus;
    private readonly ActiveModelCounter<Bullet> _activeBulletCouner;

    public ActiveBulletCounter(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _activeBulletCouner = new ActiveModelCounter<Bullet>(_eventBus);

        _eventBus.Subscribe<CreatedSignal<Bullet>>(AddBullet);

        _eventBus.Subscribe<DestroyedGameWorldSignal>(Finish);
    }

    public int Amount => _activeBulletCouner.Amount;

    private void AddBullet(CreatedSignal<Bullet> createdSignal)
    {
        _activeBulletCouner.AddActivedModel(createdSignal.Creatable);
    }

    private void Finish(DestroyedGameWorldSignal _)
    {
        _eventBus.Unsubscribe<DestroyedGameWorldSignal>(Finish);

        _eventBus.Unsubscribe<CreatedSignal<Bullet>>(AddBullet);
    }
}