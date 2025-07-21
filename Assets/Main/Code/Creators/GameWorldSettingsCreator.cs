using System;

public class GameWorldSettingsCreator
{
    private readonly GameWorldSettings _gameWorldSettings;

    public GameWorldSettingsCreator(GameWorldSettings gameWorldSettings)
    {
        _gameWorldSettings = gameWorldSettings ? gameWorldSettings : throw new ArgumentNullException(nameof(gameWorldSettings));
    }

    public GameWorldSettings PrepareGameWorldSettings(LevelSettings levelSettings)
    {
        _gameWorldSettings.SetLevelSettings(levelSettings);

        return _gameWorldSettings;
    }
}