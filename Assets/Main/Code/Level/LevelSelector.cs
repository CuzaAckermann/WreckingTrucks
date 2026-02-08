using System;

public class LevelSelector
{
    private readonly ILevelSelectionWindowsStorage _windowsStorage;

    private readonly LevelCreator _levelCreator;
    private readonly LevelSettingsStorage _storageLevelSettings;
    private readonly LevelSettingsCreator _levelSettingsCreator;

    private readonly SaveOfPlayer _saveOfPlayer;

    private int _currentIndexOfLevel;

    public LevelSelector(ILevelSelectionWindowsStorage windowsStorage,
                         LevelCreator levelCreator,
                         LevelSettingsStorage storageLevelSettings,
                         LevelSettingsCreator levelSettingsCreator,
                         SaveOfPlayer saveOfPlayer)
    {
        _windowsStorage = windowsStorage ?? throw new ArgumentNullException(nameof(windowsStorage));

        _levelCreator = levelCreator ?? throw new ArgumentNullException();
        _storageLevelSettings = storageLevelSettings ? storageLevelSettings : throw new ArgumentNullException(nameof(storageLevelSettings));
        _levelSettingsCreator = levelSettingsCreator;
        _saveOfPlayer = saveOfPlayer ? saveOfPlayer : throw new ArgumentNullException(nameof(saveOfPlayer));

        _currentIndexOfLevel = -1;

        SubscribeToWindows();
    }

    public bool HasNextLevel => _storageLevelSettings.HasNextLevelSettings(_currentIndexOfLevel);

    public bool HasPreviousLevel => _storageLevelSettings.HasPreviousLevelSettings(_currentIndexOfLevel);

    private void SubscribeToWindows()
    {
        _windowsStorage.GameSelectionWindow.StartNewGameButton.Pressed += StartNewGame;
        _windowsStorage.GameSelectionWindow.ContinueButton.Pressed += ContinueGame;

        _windowsStorage.LevelButtonsStorage.PlayButtonNonstopGame.Pressed += CreateNonstopLevel;

        for (int button = 0; _windowsStorage.LevelButtonsStorage.TryGetButton(button, out ButtonWithIndex buttonWithIndex); button++)
        {
            // Ќужна подписка только на количество кнопок равное количеству уровней
            // LevelSettingStorage.Amount

            buttonWithIndex.Pressed += CreateLevel;
        }

        _windowsStorage.PauseMenu.ResetLevelButton.Pressed += RecreateLevel;

        _windowsStorage.EndLevelWindow.ResetLevelButton.Pressed += RecreateLevel;
        _windowsStorage.EndLevelWindow.PreviousLevelButton.Pressed += CreatePreviousLevel;
        _windowsStorage.EndLevelWindow.NextLevelButton.Pressed += CreateNextLevel;
    }

    private void UnsubscribeFromWindows()
    {
        _windowsStorage.GameSelectionWindow.StartNewGameButton.Pressed -= StartNewGame;
        _windowsStorage.GameSelectionWindow.ContinueButton.Pressed -= ContinueGame;

        _windowsStorage.LevelButtonsStorage.PlayButtonNonstopGame.Pressed -= CreateNonstopLevel;

        for (int button = 0; _windowsStorage.LevelButtonsStorage.TryGetButton(button, out ButtonWithIndex buttonWithIndex); button++)
        {
            buttonWithIndex.Pressed -= CreateLevel;
        }

        _windowsStorage.PauseMenu.ResetLevelButton.Pressed -= RecreateLevel;

        _windowsStorage.EndLevelWindow.ResetLevelButton.Pressed -= RecreateLevel;
        _windowsStorage.EndLevelWindow.PreviousLevelButton.Pressed -= CreatePreviousLevel;
        _windowsStorage.EndLevelWindow.NextLevelButton.Pressed -= CreateNextLevel;
    }

    private void StartNewGame()
    {
        CreateLevel(1);
    }

    private void ContinueGame()
    {
        CreateLevel(_saveOfPlayer.CurrentLevel);
    }

    private void RecreateLevel()
    {
        CreateLevel(_currentIndexOfLevel);
    }

    private void CreateNextLevel()
    {
        CreateLevel(_currentIndexOfLevel + 1);
    }

    private void CreatePreviousLevel()
    {
        CreateLevel(_currentIndexOfLevel - 1);
    }

    private void CreateLevel(int indexOfButton)
    {
        if (indexOfButton < 0 || indexOfButton >= _storageLevelSettings.AmountLevels)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfButton));
        }

        _currentIndexOfLevel = indexOfButton;
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