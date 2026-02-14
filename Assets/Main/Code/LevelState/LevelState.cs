using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelState
{
    private readonly EventBus _eventBus;

    private Level _level;

    public LevelState(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _eventBus.Subscribe<CreatedSignal<Level>>(SetLevel);

        _eventBus.Subscribe<ClearedSignal<ApplicationSignal>>(Finish);
    }

    public void Enter()
    {
        _level?.Enable();
    }

    private void Finish(ClearedSignal<ApplicationSignal> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<ApplicationSignal>>(Finish);

        _eventBus.Unsubscribe<CreatedSignal<Level>>(SetLevel);
    }

    private void SetLevel(CreatedSignal<Level> createdSignal)
    {
        DestroyLevel();

        _level = createdSignal.Creatable;
    }

    private void DestroyLevel()
    {
        _level?.Disable();
        _level?.Clear();

        _level = null;
    }
}