using System;

public abstract class TargetActionUpdater<T> : ITickable where T : ITargetAction
{
    private readonly EventBus _eventBus;

    private readonly LockedStorage<T> _storage;

    private bool _isRunning;

    public TargetActionUpdater(EventBus eventBus, int capacity)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _storage = new LockedStorage<T>(capacity);

        _eventBus.Subscribe<ClearedSignal<GameSignalEmitter>>(Clear);
        _eventBus.Subscribe<EnabledSignal<GameSignalEmitter>>(Enable);
        _eventBus.Subscribe<DisabledSignal<GameSignalEmitter>>(Disable);

        _isRunning = false;
    }

    public event Action<IDestroyable> Destroyed;

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void Tick(float deltaTime)
    {
        if (_storage.HasActive() == false)
        {
            return;
        }

        _storage.Lock();

        foreach (T targetAction in _storage.GetClearedActive())
        {
            targetAction.DoStep(deltaTime);
        }

        _storage.Unlock();
    }

    protected abstract T GetTargetAction(Model model);

    private void Clear(ClearedSignal<GameSignalEmitter> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<GameSignalEmitter>>(Clear);
        _eventBus.Unsubscribe<EnabledSignal<GameSignalEmitter>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<GameSignalEmitter>>(Disable);
    }

    private void Enable(EnabledSignal<GameSignalEmitter> _)
    {
        if (_isRunning == false)
        {
            _isRunning = true;

            _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);

            Activated?.Invoke(this);
        }
    }

    private void Disable(DisabledSignal<GameSignalEmitter> _)
    {
        if (_isRunning)
        {
            _isRunning = false;

            Deactivated?.Invoke(this);

            _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);
        }
    }

    private void OnModelCreated(CreatedSignal<Model> modelSignal)
    {
        _storage.Register(GetTargetAction(modelSignal.Creatable));
    }
}