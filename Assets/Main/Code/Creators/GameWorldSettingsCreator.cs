using System;

public class GameWorldSettingsCreator
{
    private readonly GameWorldSettings _gameWorldSettings;
    private readonly PlacementSettings _placementSettings;
    private readonly PathSettings _pathSettings;

    public GameWorldSettingsCreator(GameWorldSettings gameWorldSettings,
                                    PlacementSettings placementSettings,
                                    PathSettings pathSettings)
    {
        _gameWorldSettings = gameWorldSettings ? gameWorldSettings : throw new ArgumentNullException(nameof(gameWorldSettings));
        _placementSettings = placementSettings ? placementSettings : throw new ArgumentNullException(nameof(placementSettings));
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
    }

    public GameWorldSettings PrepareGameWorldSettings(LevelSettings levelSettings)
    {
        _gameWorldSettings.SetLevelSettings(_placementSettings,
                                            _pathSettings,
                                            levelSettings);

        return _gameWorldSettings;
    }
}