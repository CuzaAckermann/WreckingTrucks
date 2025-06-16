using System;

public class GameWorldCreator
{
    private readonly ISpaceCreator _blocksSpaceCreator;
    private readonly ISpaceCreator _trucksSpaceCreator;
    private readonly IRoadSpaceCreator _roadSpaceCreator;

    public GameWorldCreator(ISpaceCreator blocksSpaceCreator,
                            ISpaceCreator trucksSpaceCreator,
                            IRoadSpaceCreator roadSpaceCreator)
    {
        _blocksSpaceCreator = blocksSpaceCreator ?? throw new ArgumentNullException(nameof(blocksSpaceCreator));
        _trucksSpaceCreator = trucksSpaceCreator ?? throw new ArgumentNullException(nameof(trucksSpaceCreator));
        _roadSpaceCreator = roadSpaceCreator ?? throw new ArgumentNullException(nameof(roadSpaceCreator));
    }

    public GameWorld CreateGameWorld(LevelSettings levelSettings)
    {
        GameWorld gameWorld = new GameWorld(_blocksSpaceCreator.CreateSpace(levelSettings.BlocksSpaceSettings),
                                            _trucksSpaceCreator.CreateSpace(levelSettings.TrucksSpaceSettings),
                                            _roadSpaceCreator.CreateRoadSpace());

        gameWorld.Prepare(levelSettings);

        return gameWorld;
    }
}