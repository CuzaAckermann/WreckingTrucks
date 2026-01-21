using System;

public class EndLevelDefiner
{
    private readonly EventBus _eventBus;

    private readonly Dispencer _dispencer;
    private readonly ActiveModelCounter<Truck> _activeTruckCounter;
    private readonly ActiveBulletCounter _activeBulletCounter;

    private bool _isEnabled;

    public EndLevelDefiner(EventBus eventBus, Dispencer dispencer)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _dispencer = dispencer ?? throw new ArgumentNullException(nameof(dispencer));

        _activeTruckCounter = new ActiveModelCounter<Truck>(eventBus);
        _activeBulletCounter = new ActiveBulletCounter(eventBus);

        SubscribeMain();
    }

    private void Clear(ClearedSignal<Level> _)
    {
        UnsubscribeAll();

        _isEnabled = false;
    }

    private void Complete(BlockFieldWastedSignal _)
    {
        UnsubscribeAll();

        _eventBus.Invoke(new CompletedSignal<Level>());
    }

    private void WaitLastChanse(CartrigeBoxFieldWastedSignal _)
    {
        if (_isEnabled)
        {
            return;
        }

        _isEnabled = true;

        _eventBus.Subscribe<RecordAppearedSignal>(WaitMainSubscription);

        if (_activeTruckCounter.Amount > 0)
        {
            _eventBus.Subscribe<ActiveModelIsEmptySignal<Truck>>(WaitTrucksToFinish);

            return;
        }

        if (_activeBulletCounter.Amount > 0)
        {
            _eventBus.Subscribe<ActiveModelIsEmptySignal<Bullet>>(WaitBulletsToFinish);

            return;
        }

        UnsubscribeAll();

        _eventBus.Invoke(new FailedSignal<Level>());
    }

    private void WaitTrucksToFinish(ActiveModelIsEmptySignal<Truck> _)
    {
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Truck>>(WaitTrucksToFinish);

        if (_activeBulletCounter.Amount > 0)
        {
            _eventBus.Subscribe<ActiveModelIsEmptySignal<Bullet>>(WaitBulletsToFinish);

            return;
        }

        DetectFailedSignal();
    }

    private void WaitBulletsToFinish(ActiveModelIsEmptySignal<Bullet> _)
    {
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Bullet>>(WaitBulletsToFinish);

        DetectFailedSignal();
    }

    private void DetectFailedSignal()
    {
        if (_dispencer.Amount > 0)
        {
            return;
        }

        UnsubscribeAll();

        _eventBus.Invoke(new FailedSignal<Level>());
    }

    private void WaitMainSubscription(RecordAppearedSignal _)
    {
        UnsubscribeSecondary();
    }

    private void SubscribeMain()
    {
        _eventBus.Subscribe<ClearedSignal<Level>>(Clear);
        _eventBus.Subscribe<BlockFieldWastedSignal>(Complete);
        _eventBus.Subscribe<CartrigeBoxFieldWastedSignal>(WaitLastChanse);
    }

    private void UnsubscribeAll()
    {
        UnsubscribeMain();
        UnsubscribeSecondary();
    }

    private void UnsubscribeMain()
    {
        _eventBus.Unsubscribe<ClearedSignal<Level>>(Clear);
        _eventBus.Unsubscribe<BlockFieldWastedSignal>(Complete);
        _eventBus.Unsubscribe<CartrigeBoxFieldWastedSignal>(WaitLastChanse);
    }

    private void UnsubscribeSecondary()
    {
        _eventBus.Unsubscribe<RecordAppearedSignal>(WaitMainSubscription);
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Truck>>(WaitTrucksToFinish);
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Bullet>>(WaitBulletsToFinish);
    }
}