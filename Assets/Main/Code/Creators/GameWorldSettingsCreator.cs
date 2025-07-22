using System;

public class GameWorldSettingsCreator
{
    private readonly GameWorldSettings _gameWorldSettings;

    public GameWorldSettingsCreator(GameWorldSettings gameWorldSettings)
    {
        _gameWorldSettings = gameWorldSettings ? gameWorldSettings : throw new ArgumentNullException(nameof(gameWorldSettings));
    }

    public GameWorldSettings PrepareGameWorldSettings(PlacementSettings placementSettings,
                                                      PathSettings pathSettings,
                                                      LevelSettings levelSettings)
    {
        _gameWorldSettings.SetLevelSettings(placementSettings,
                                            pathSettings,
                                            levelSettings);

        return _gameWorldSettings;
    }
}