using System;
using System.Collections.Generic;

public class LevelSelector : IApplicationAbility
{
    private readonly IWindowStorage _windowsStorage;

    private readonly LevelCreator _levelCreator;
    private readonly LevelSettingsStorage _storageLevelSettings;
    private readonly LevelSettingsCreator _levelSettingsCreator;

    private readonly SaveOfPlayer _saveOfPlayer;

    private readonly Dictionary<BaseUiButton, Action> _buttonHandlers;

    private int _currentIndexOfLevel;

    public LevelSelector(IWindowStorage windowsStorage,
                         LevelCreator levelCreator,
                         LevelSettingsStorage storageLevelSettings,
                         LevelSettingsCreator levelSettingsCreator,
                         SaveOfPlayer saveOfPlayer)
    {
        Validator.ValidateNotNull(windowsStorage,
                                  levelCreator,
                                  storageLevelSettings,
                                  levelSettingsCreator,
                                  saveOfPlayer);

        _windowsStorage = windowsStorage;
        _levelCreator = levelCreator;
        _storageLevelSettings = storageLevelSettings;
        _levelSettingsCreator = levelSettingsCreator;
        _saveOfPlayer = saveOfPlayer;

        if (_windowsStorage.TryGet(out GameSelectionWindow gameSelectionWindow) == false)
        {
            throw new InvalidOperationException();
        }

        if (_windowsStorage.TryGet(out LevelButtonsStorage levelButtonsStorage) == false)
        {
            throw new InvalidOperationException();
        }

        if (_windowsStorage.TryGet(out PauseMenu pauseMenu) == false)
        {
            throw new InvalidOperationException();
        }

        if (_windowsStorage.TryGet(out EndLevelWindow endLevelWindow) == false)
        {
            throw new InvalidOperationException();
        }

        _buttonHandlers = new Dictionary<BaseUiButton, Action>
        {
            { gameSelectionWindow.StartNewGameButton, () => CreateLevel(0) },
            { gameSelectionWindow.ContinueButton, () => CreateLevel(_saveOfPlayer.CurrentLevel) },
            { pauseMenu.ResetLevelButton, () => CreateLevel(_currentIndexOfLevel) },
            { endLevelWindow.ResetLevelButton, () => CreateLevel(_currentIndexOfLevel) },
            { endLevelWindow.PreviousLevelButton, () => CreateLevel(_currentIndexOfLevel - 1) },
            { endLevelWindow.NextLevelButton, () => CreateLevel(_currentIndexOfLevel + 1) },
            { levelButtonsStorage.PlayButtonNonstopGame, () => CreateNonstopLevel() }
        };

        // Нужно отключать кнопки, которые соответствуют недоступным уровням

        for (int i = 0; levelButtonsStorage.TryGetButton(i, out ButtonWithIndex buttonWithIndex); i++)
        {
            ButtonWithIndex button = buttonWithIndex;
            _buttonHandlers.Add(button, () => CreateLevel(button.Index));
        }

        _currentIndexOfLevel = -1;
    }

    public bool HasNextLevel => _storageLevelSettings.HasNextLevelSettings(_currentIndexOfLevel);

    public bool HasPreviousLevel => _storageLevelSettings.HasPreviousLevelSettings(_currentIndexOfLevel);

    public void Start()
    {
        foreach (var pair in _buttonHandlers)
        {
            pair.Key.Pressed += pair.Value;
        }
    }

    public void Finish()
    {
        foreach (var pair in _buttonHandlers)
        {
            pair.Key.Pressed -= pair.Value;
        }
    }

    private void CreateLevel(int index)
    {
        Validator.ValidateMin(index, 0, false);
        Validator.ValidateMax(index, _storageLevelSettings.AmountLevels, true);

        _currentIndexOfLevel = index;
        _saveOfPlayer.SetCurrentLevel(_currentIndexOfLevel);

        LevelSettings levelSettings = _storageLevelSettings.GetLevelSettings(_currentIndexOfLevel);

        CommonLevelSettings commonLevelSettings = _levelSettingsCreator.PrepareGameWorldSettings(levelSettings);

        _levelCreator.CreateLevel(commonLevelSettings).Enable();
    }

    private void CreateNonstopLevel()
    {
        _levelCreator.CreateNonstopLevel(_levelSettingsCreator.GetGameWorldSettings()).Enable();
    }
}