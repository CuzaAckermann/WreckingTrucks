using System;

public class GameWorldCreator
{
    private readonly ISpaceCreator _blocksSpaceCreator;
    private readonly TrucksSpaceCreator _trucksSpaceCreator;
    private readonly RoadSpaceCreator _roadSpaceCreator;
    private readonly ShootingSpaceCreator _shootingSpaceCreator;

    public GameWorldCreator(ISpaceCreator blocksSpaceCreator,
                            TrucksSpaceCreator trucksSpaceCreator,
                            RoadSpaceCreator roadSpaceCreator,
                            ShootingSpaceCreator shootingSpaceCreator)
    {
        _blocksSpaceCreator = blocksSpaceCreator ?? throw new ArgumentNullException(nameof(blocksSpaceCreator));
        _trucksSpaceCreator = trucksSpaceCreator ?? throw new ArgumentNullException(nameof(trucksSpaceCreator));
        _roadSpaceCreator = roadSpaceCreator ?? throw new ArgumentNullException(nameof(roadSpaceCreator));
        _shootingSpaceCreator = shootingSpaceCreator ?? throw new ArgumentNullException(nameof(shootingSpaceCreator));
    }

    public GameWorld CreateGameWorld(LevelSettings levelSettings)
    {
        GameWorld gameWorld = new GameWorld(_blocksSpaceCreator.CreateSpace(levelSettings.BlocksSpaceSettings),
                                            _trucksSpaceCreator.CreateTruckSpace(levelSettings.TrucksSpaceSettings),
                                            _roadSpaceCreator.CreateRoadSpace(),
                                            _shootingSpaceCreator.CreateShootingSpace());

        gameWorld.Prepare(levelSettings);

        return gameWorld;
    }
}