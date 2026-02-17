using System;

public abstract class TargetActionUpdater<T> : ITickable, IAbility where T : ITargetAction
{
    private readonly EventBus _eventBus;

    private readonly TargetActionLockedStorage<T> _storage;

    private bool _isRunning;

    public TargetActionUpdater(EventBus eventBus, int capacity)
    {
        Validator.ValidateNotNull(eventBus);

        _eventBus = eventBus;

        _storage = new TargetActionLockedStorage<T>(capacity);

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

        foreach (ITargetAction targetAction in _storage.GetClearedActive())
        {
            targetAction.DoStep(deltaTime);
        }

        _storage.Unlock();
    }

    public void Start()
    {
        if (_isRunning == false)
        {
            _isRunning = true;

            _eventBus.Subscribe<CreatedSignal<Model>>(OnModelCreated);

            Activated?.Invoke(this);
        }
    }

    public void Finish()
    {
        if (_isRunning)
        {
            _isRunning = false;

            Deactivated?.Invoke(this);

            _eventBus.Unsubscribe<CreatedSignal<Model>>(OnModelCreated);
        }
    }

    protected abstract T GetTargetAction(Model model);

    private void OnModelCreated(CreatedSignal<Model> modelSignal)
    {
        _storage.Register(GetTargetAction(modelSignal.Creatable));
    }
}