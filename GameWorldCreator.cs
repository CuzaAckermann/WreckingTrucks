using System;

public class GameWorldCreator
{
    private ISpaceCreator _spaceCreator;

    public GameWorldCreator(ISpaceCreator spaceCreator)
    {
        _spaceCreator = spaceCreator ?? throw new ArgumentNullException(nameof(spaceCreator));
    }

    public GameWorld CreateGameWorld(LevelSettings levelSettings)
    {
        GameWorld gameWorld = new GameWorld(_spaceCreator.CreateBlocksSpace(levelSettings));
        gameWorld.Prepare(levelSettings);

        return gameWorld;
    }
}