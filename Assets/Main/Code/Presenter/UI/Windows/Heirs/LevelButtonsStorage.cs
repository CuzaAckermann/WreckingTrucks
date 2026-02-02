using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonsStorage : WindowOfState<LevelSelectionState>
{
    [SerializeField] private GameButton _returnButton;
    [SerializeField] private GameButton _nonstopGameButton;
    [SerializeField] private List<ButtonWithIndex> _levelButtons;

    private bool _isInitialized;

    public void Init(LevelSelectionState levelSelectionState, int amountLevels)
    {
        if (_isInitialized)
        {
            throw new InvalidOperationException($"Already initialized");
        }

        if (amountLevels <= 0 || amountLevels > _levelButtons.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(amountLevels));
        }

        base.Init(levelSelectionState);

        for (int i = 0; i < amountLevels; i++)
        {
            _levelButtons[i].Initailize(i);
        }

        _isInitialized = true;
    }

    public GameButton ReturnButton => _returnButton;

    public GameButton NonstopGameButton => _nonstopGameButton;

    public bool TryGetButton(int index, out ButtonWithIndex buttonWithIndex)
    {
        buttonWithIndex = null;

        if (index < 0 || index >= _levelButtons.Count)
        {
            return false;
        }
        
        buttonWithIndex = _levelButtons[index];

        return true;
    }
}