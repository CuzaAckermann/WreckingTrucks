using System;

public class GameWorldCreator
{
    private ISpaceCreator _blocksSpaceCreator;
    private ISpaceCreator _trucksSpaceCreator;

    public GameWorldCreator(ISpaceCreator blocksSpaceCreator,
                            ISpaceCreator trucksSpaceCreator)
    {
        _blocksSpaceCreator = blocksSpaceCreator ?? throw new ArgumentNullException(nameof(blocksSpaceCreator));
        _trucksSpaceCreator = trucksSpaceCreator ?? throw new ArgumentNullException(nameof(trucksSpaceCreator));
    }

    public GameWorld CreateGameWorld(LevelSettings levelSettings)
    {
        GameWorld gameWorld = new GameWorld(_blocksSpaceCreator.CreateSpace(levelSettings),
                                            _trucksSpaceCreator.CreateSpace(levelSettings));
        gameWorld.Prepare(levelSettings.FillingCardWithBlocks, levelSettings.FillingCardWithTrucks);

        return gameWorld;
    }
}