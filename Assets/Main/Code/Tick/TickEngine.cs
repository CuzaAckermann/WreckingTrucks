using System;
using System.Collections.Generic;

public class TickEngine : IAbility
{
    private readonly IAmount _deltaTime;

    private readonly EventBus _eventBus;

    private readonly TickableLockedStorage _storage;

    private readonly List<Type> _addableTypes;

    private bool _isPaused;

    public TickEngine(EventBus eventBus,
                      IAmount deltaTime,
                      List<Type> addableTypes)
    {
        Validator.ValidateNotNull(eventBus, deltaTime, addableTypes);

        _eventBus = eventBus;
        _deltaTime = deltaTime;

        _storage = new TickableLockedStorage(100);

        _addableTypes = addableTypes;

        _isPaused = true;
    }

    public void Start()
    {
        Continue();

        _eventBus.Subscribe<CreatedSignal<IDestroyable>>(OnCreated);

        _deltaTime.Changed += Tick;
    }

    public void Finish()
    {
        Pause();

        _eventBus.Unsubscribe<CreatedSignal<IDestroyable>>(OnCreated);

        _deltaTime.Changed -= Tick;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Continue()
    {
        _isPaused = false;
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

    private void OnCreated(CreatedSignal<IDestroyable> createdSignal)
    {
        for (int i = 0; i < _addableTypes.Count; i++)
        {
            if (createdSignal.Creatable.GetType() != _addableTypes[i])
            {
                continue;
            }

            if (Validator.IsRequiredType(createdSignal.Creatable, out ITickable tickable) == false)
            {
                continue;
            }

            _storage.Register(tickable);

            return;
        }
    }
}