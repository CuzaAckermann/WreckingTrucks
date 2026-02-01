using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PostLevelAnalyzer
{
    private readonly EventBus _eventBus;
    private LevelSelector _levelSelector;

    public PostLevelAnalyzer(EventBus eventBus, LevelSelector levelSelector)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _levelSelector = levelSelector ?? throw new ArgumentNullException(nameof(levelSelector));
    }

    #region Level Resoult Subscribes / Unsubscribes
    private void SubscribeToLevelResoult()
    {
        _eventBus.Subscribe<CompletedSignal<Level>>(OnLevelCompleted);
        _eventBus.Subscribe<FailedSignal<Level>>(OnLevelFailed);
    }

    private void UnsubscribeFromLevelResoult()
    {
        _eventBus.Unsubscribe<CompletedSignal<Level>>(OnLevelCompleted);
        _eventBus.Unsubscribe<FailedSignal<Level>>(OnLevelFailed);
    }
    #endregion

    #region Level Resoult Handlers
    private void OnLevelCompleted(CompletedSignal<Level> _)
    {
        // корректировка Здесь кнопка следующего уровня доступна
        //_endLevelWindow.SetLevelNavigationState(_levelSelector.HasNextLevel, _levelSelector.HasPreviousLevel);
    }

    private void OnLevelFailed(FailedSignal<Level> _)
    {
        // корректировка Здесь кнопка следующего уровня не доступна
        //_endLevelWindow.SetLevelNavigationState(false, _levelSelector.HasPreviousLevel);
    }
    #endregion
}