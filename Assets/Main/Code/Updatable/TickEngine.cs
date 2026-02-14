using System;
using System.Collections.Generic;

public class TickEngine
{
    private readonly EventBus _eventBus;
    private readonly IAmount _deltaTime;

    private readonly List<ITickableCreator> _tickableCreators;

    private readonly TickableLockedStorage _storage;
    
    private bool _isPaused;

    public TickEngine(EventBus eventBus, IAmount deltaTime)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        Validator.ValidateNotNull(deltaTime);
        _deltaTime = deltaTime;

        _tickableCreators = new List<ITickableCreator>();
        _storage = new TickableLockedStorage(100);

        _isPaused = true;

        _eventBus.Subscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Subscribe<EnabledSignal<ApplicationSignal>>(Start);

        _deltaTime.ValueChanged += Tick;
    }

    public void AddTickableCreator(ITickableCreator tickableCreator)
    {
        if (tickableCreator == null)
        {
            throw new ArgumentNullException(nameof(tickableCreator));
        }

        if (_tickableCreators.Contains(tickableCreator))
        {
            throw new InvalidOperationException($"{nameof(tickableCreator)} is already added");
        }

        _tickableCreators.Add(tickableCreator);

        SubscribeToTickableCreator(tickableCreator);
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Continue()
    {
        _isPaused = false;
    }

    private void Clear(ClearedSignal<ApplicationSignal> _)
    {
        for (int tickableCreator = 0; tickableCreator < _tickableCreators.Count; tickableCreator++)
        {
            UnsubscribeFromTickableCreator(_tickableCreators[tickableCreator]);
        }

        _eventBus.Unsubscribe<ClearedSignal<ApplicationSignal>>(Clear);

        _eventBus.Unsubscribe<EnabledSignal<ApplicationSignal>>(Start);

        _deltaTime.ValueChanged -= Tick;
    }

    private void Start(EnabledSignal<ApplicationSignal> _)
    {
        Continue();
    }

    private void Tick(float deltaTime)
    {
        if (_isPaused)
        {
            return;
        }

        if (_storage.HasActive() == false)
        {
            return;
        }

        _storage.Lock();

        foreach (ITickable tickable in _storage.GetClearedActive())
        {
            tickable.Tick(deltaTime);
        }

        _storage.Unlock();
    }

    private void SubscribeToTickableCreator(ITickableCreator tickableCreator)
    {
        tickableCreator.TickableCreated += OnCreated;
    }

    private void UnsubscribeFromTickableCreator(ITickableCreator tickableCreator)
    {
        tickableCreator.TickableCreated -= OnCreated;
    }

    private void OnCreated(ITickable tickable)
    {
        _storage.Register(tickable);
    }
}