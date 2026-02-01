using System;

public class LevelSelector
{
    private readonly ILevelSelectionInformer _informer;

    private readonly LevelCreator _levelCreator;
    private readonly LevelSettingsStorage _storageLevelSettings;
    private readonly LevelSettingsCreator _levelSettingsCreator;

    private int _currentIndexOfLevel;

    public LevelSelector(ILevelSelectionInformer levelSelectionInformer,
                         LevelCreator levelCreator,
                         LevelSettingsStorage storageLevelSettings,
                         LevelSettingsCreator levelSettingsCreator)
    {
        _informer = levelSelectionInformer ?? throw new ArgumentNullException(nameof(levelSelectionInformer));

        _levelCreator = levelCreator ?? throw new ArgumentNullException();
        _storageLevelSettings = storageLevelSettings ? storageLevelSettings : throw new ArgumentNullException(nameof(storageLevelSettings));
        _levelSettingsCreator = levelSettingsCreator;
        _currentIndexOfLevel = -1;

        SubscribeToInformer();
    }

    public bool HasNextLevel => _storageLevelSettings.HasNextLevelSettings(_currentIndexOfLevel);

    public bool HasPreviousLevel => _storageLevelSettings.HasPreviousLevelSettings(_currentIndexOfLevel);

    private void SubscribeToInformer()
    {
        _informer.IndexSelected += CreateLevel;
        _informer.NextSelected += CreateNextLevel;
        _informer.PreviousSelected += CreatePreviousLevel;
        _informer.RecreateSelected += Recreate;
        _informer.NonstopSelected += CreateNonstopLevel;
    }

    private void UnsubscribeFromInformer()
    {
        _informer.IndexSelected -= CreateLevel;
        _informer.NextSelected -= CreateNextLevel;
        _informer.PreviousSelected -= CreatePreviousLevel;
        _informer.RecreateSelected -= Recreate;
        _informer.NonstopSelected -= CreateNonstopLevel;
    }

    private void Recreate()
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

    private void CreateLevel(int indexOfLevel)
    {
        if (indexOfLevel < 0 || indexOfLevel >= _storageLevelSettings.AmountLevels)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLevel));
        }

        _currentIndexOfLevel = indexOfLevel;

        LevelSettings levelSettings = _storageLevelSettings.GetLevelSettings(_currentIndexOfLevel);

        CommonLevelSettings commonLevelSettings = _levelSettingsCreator.PrepareGameWorldSettings(levelSettings);

        _levelCreator.CreateLevelGame(commonLevelSettings);
    }

    private void CreateNonstopLevel()
    {
        _levelCreator.CreateNonstopGame(_levelSettingsCreator.GetGameWorldSettings());
    }
}