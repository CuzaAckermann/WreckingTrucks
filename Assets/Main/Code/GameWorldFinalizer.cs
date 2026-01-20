using System;

public class GameWorldFinalizer
{
    private readonly EventBus _eventBus;

    private readonly Dispencer _dispencer;
    private readonly ActiveModelCounter<Truck> _activeTruckCounter;
    private readonly ActiveBulletCounter _activeBulletCounter;

    private bool _isEnabled;

    public GameWorldFinalizer(EventBus eventBus, Dispencer dispencer)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _dispencer = dispencer ?? throw new ArgumentNullException(nameof(dispencer));

        _activeTruckCounter = new ActiveModelCounter<Truck>(eventBus);
        _activeBulletCounter = new ActiveBulletCounter(eventBus);
    }

    public void Enable()
    {
        if (_isEnabled)
        {
            return;
        }

        _isEnabled = true;

        if (_activeTruckCounter.Amount == 0 && _activeBulletCounter.Amount == 0)
        {
            _eventBus.Invoke(new FailedSignal<GameWorld>());
        }

        if (_activeTruckCounter.Amount > 0)
        {
            _eventBus.Subscribe<ActiveModelIsEmptySignal<Truck>>(OnActivedTrucksIsEmpty);
        }
        else if (_activeBulletCounter.Amount > 0)
        {
            _eventBus.Subscribe<ActiveModelIsEmptySignal<Bullet>>(OnActivedBulletIsEmpty);
        }

        _eventBus.Subscribe<RecordAppearedSignal>(OnRecordAppeared);
    }

    public void Disable()
    {
        _eventBus.Unsubscribe<RecordAppearedSignal>(OnRecordAppeared);

        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Truck>>(OnActivedTrucksIsEmpty);
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Bullet>>(OnActivedBulletIsEmpty);

        _isEnabled = false;
    }

    private void OnActivedTrucksIsEmpty(ActiveModelIsEmptySignal<Truck> _)
    {
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Truck>>(OnActivedTrucksIsEmpty);

        if (_activeBulletCounter.Amount > 0)
        {
            _eventBus.Subscribe<ActiveModelIsEmptySignal<Bullet>>(OnActivedBulletIsEmpty);
        }
        else if (_dispencer.Amount == 0)
        {
            _eventBus.Unsubscribe<RecordAppearedSignal>(OnRecordAppeared);

            _eventBus.Invoke(new FailedSignal<GameWorld>());
        }
    }

    private void OnActivedBulletIsEmpty(ActiveModelIsEmptySignal<Bullet> _)
    {
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Bullet>>(OnActivedBulletIsEmpty);

        if (_dispencer.Amount == 0)
        {
            _eventBus.Unsubscribe<RecordAppearedSignal>(OnRecordAppeared);

            _eventBus.Invoke(new FailedSignal<GameWorld>());
        }
    }

    private void OnRecordAppeared(RecordAppearedSignal _)
    {
        _eventBus.Unsubscribe<RecordAppearedSignal>(OnRecordAppeared);

        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Truck>>(OnActivedTrucksIsEmpty);
        _eventBus.Unsubscribe<ActiveModelIsEmptySignal<Bullet>>(OnActivedBulletIsEmpty);

        _eventBus.Invoke(new ContinuedSignal<GameWorld>());
    }
}