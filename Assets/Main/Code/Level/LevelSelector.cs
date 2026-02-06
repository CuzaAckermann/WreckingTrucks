using System;

public class LevelSelector
{
    private readonly ILevelSelectionWindowsStorage _windowsStorage;

    private readonly LevelCreator _levelCreator;
    private readonly LevelSettingsStorage _storageLevelSettings;
    private readonly LevelSettingsCreator _levelSettingsCreator;

    private int _currentIndexOfLevel;

    public LevelSelector(ILevelSelectionWindowsStorage windowsStorage,
                         LevelCreator levelCreator,
                         LevelSettingsStorage storageLevelSettings,
                         LevelSettingsCreator levelSettingsCreator)
    {
        _windowsStorage = windowsStorage ?? throw new ArgumentNullException(nameof(windowsStorage));

        _levelCreator = levelCreator ?? throw new ArgumentNullException();
        _storageLevelSettings = storageLevelSettings ? storageLevelSettings : throw new ArgumentNullException(nameof(storageLevelSettings));
        _levelSettingsCreator = levelSettingsCreator;
        _currentIndexOfLevel = -1;

        SubscribeToWindows();
    }

    public bool HasNextLevel => _storageLevelSettings.HasNextLevelSettings(_currentIndexOfLevel);

    public bool HasPreviousLevel => _storageLevelSettings.HasPreviousLevelSettings(_currentIndexOfLevel);

    private void SubscribeToWindows()
    {
        _windowsStorage.LevelButtonsStorage.PlayButtonNonstopGame.Pressed += CreateNonstopLevel;

        for (int button = 0; _windowsStorage.LevelButtonsStorage.TryGetButton(button, out ButtonWithNumber buttonWithIndex); button++)
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
        _windowsStorage.LevelButtonsStorage.PlayButtonNonstopGame.Pressed -= CreateNonstopLevel;

        for (int button = 0; _windowsStorage.LevelButtonsStorage.TryGetButton(button, out ButtonWithNumber buttonWithIndex); button++)
        {
            buttonWithIndex.Pressed -= CreateLevel;
        }

        _windowsStorage.PauseMenu.ResetLevelButton.Pressed -= RecreateLevel;

        _windowsStorage.EndLevelWindow.ResetLevelButton.Pressed -= RecreateLevel;
        _windowsStorage.EndLevelWindow.PreviousLevelButton.Pressed -= CreatePreviousLevel;
        _windowsStorage.EndLevelWindow.NextLevelButton.Pressed -= CreateNextLevel;
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

    private void CreateLevel(int numberOfButton)
    {
        numberOfButton--;

        if (numberOfButton < 0 || numberOfButton >= _storageLevelSettings.AmountLevels)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfButton));
        }

        _currentIndexOfLevel = numberOfButton;

        LevelSettings levelSettings = _storageLevelSettings.GetLevelSettings(_currentIndexOfLevel);

        CommonLevelSettings commonLevelSettings = _levelSettingsCreator.PrepareGameWorldSettings(levelSettings);

        _levelCreator.CreateLevel(commonLevelSettings).Enable();
    }

    private void CreateNonstopLevel()
    {
        _levelCreator.CreateNonstopLevel(_levelSettingsCreator.GetGameWorldSettings()).Enable();
    }
}