using System;
using System.Collections.Generic;

public class TickEngine
{
    private readonly ApplicationStateStorage _applicationStateStorage;
    private readonly IAmount _deltaTime;

    private readonly List<ITickableCreator> _tickableCreators;

    private readonly TickableLockedStorage _storage;
    
    private bool _isPaused;

    public TickEngine(ApplicationStateStorage applicationStateStorage, IAmount deltaTime)
    {
        Validator.ValidateNotNull(applicationStateStorage, deltaTime);

        _applicationStateStorage = applicationStateStorage;
        _deltaTime = deltaTime;

        _tickableCreators = new List<ITickableCreator>();
        _storage = new TickableLockedStorage(100);

        _isPaused = true;

        _applicationStateStorage.FinishApplicationState.Triggered += Clear;

        _applicationStateStorage.PrepareApplicationState.Triggered += Start;

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

    private void Clear()
    {
        for (int tickableCreator = 0; tickableCreator < _tickableCreators.Count; tickableCreator++)
        {
            UnsubscribeFromTickableCreator(_tickableCreators[tickableCreator]);
        }

        _applicationStateStorage.FinishApplicationState.Triggered -= Clear;

        _applicationStateStorage.PrepareApplicationState.Triggered -= Start;

        _deltaTime.ValueChanged -= Tick;
    }

    private void Start()
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