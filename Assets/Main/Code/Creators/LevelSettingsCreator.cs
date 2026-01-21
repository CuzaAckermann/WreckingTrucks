using System;

public class LevelSettingsCreator
{
    private readonly CommonLevelSettings _gameWorldSettings;

    public LevelSettingsCreator(CommonLevelSettings gameWorldSettings)
    {
        _gameWorldSettings = gameWorldSettings ? gameWorldSettings : throw new ArgumentNullException(nameof(gameWorldSettings));
    }

    public CommonLevelSettings PrepareGameWorldSettings(LevelSettings levelSettings)
    {
        _gameWorldSettings.SetLevelSettings(levelSettings);

        return _gameWorldSettings;
    }

    public CommonLevelSettings GetGameWorldSettings()
    {
        return _gameWorldSettings;
    }
}