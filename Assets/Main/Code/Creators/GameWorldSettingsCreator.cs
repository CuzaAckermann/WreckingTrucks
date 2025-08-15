using System;

public class GameWorldSettingsCreator
{
    private readonly GameWorldSettings _gameWorldSettings;
    private readonly PlacementSettings _placementSettings;

    public GameWorldSettingsCreator(GameWorldSettings gameWorldSettings,
                                    PlacementSettings placementSettings)
    {
        _gameWorldSettings = gameWorldSettings ? gameWorldSettings : throw new ArgumentNullException(nameof(gameWorldSettings));
        _placementSettings = placementSettings ? placementSettings : throw new ArgumentNullException(nameof(placementSettings));
    }

    public GameWorldSettings PrepareGameWorldSettings(LevelSettings levelSettings)
    {
        _gameWorldSettings.SetLevelSettings(_placementSettings,
                                            levelSettings);

        return _gameWorldSettings;
    }
}