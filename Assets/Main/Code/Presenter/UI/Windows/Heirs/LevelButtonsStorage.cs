using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonsStorage : Window<LevelSelectionState>
{
    [SerializeField] private List<LevelButton> _levelButtons;

    private bool _isInitialized;

    public event Action<int> LevelActivated;

    public void Initailize(LevelSelectionState levelSelectionState, int amountLevels)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"Already initialized");
        }

        if (amountLevels <= 0 || amountLevels > _levelButtons.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(amountLevels));
        }

        base.Initialize(levelSelectionState);

        for (int i = 0; i < amountLevels; i++)
        {
            _levelButtons[i].Initailize(i);
            _levelButtons[i].Pressed += OnLevelActivated;
        }

        _isInitialized = true;
    }

    protected override void SubscribeToInteractables(LevelSelectionState gameState)
    {
        if (_isInitialized)
        {
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                _levelButtons[i].Pressed += OnLevelActivated;
            }
        }
    }

    protected override void UnsubscribeFromInteractables(LevelSelectionState gameState)
    {
        if (_isInitialized)
        {
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                _levelButtons[i].Pressed -= OnLevelActivated;
            }
        }
    }

    private void OnLevelActivated(int indexOfLevel)
    {
        LevelActivated?.Invoke(indexOfLevel);
    }
}