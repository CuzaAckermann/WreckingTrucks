using System;
using System.Collections.Generic;

public class GameWorld
{
    private Space _blocksSpace;
    private Space _trucksSpace;
    private Space _ammunitionKits;

    public GameWorld(Space blocksSpace,
                     Space trucksSpace)
    {
        _blocksSpace = blocksSpace ?? throw new ArgumentNullException(nameof(blocksSpace));
        _trucksSpace = trucksSpace ?? throw new ArgumentNullException(nameof(trucksSpace));
    }

    public event Action LevelCompleted;
    public event Action LevelFailed;

    public void Prepare(IEnumerable<Row> rowsWithBlocks, IEnumerable<Row> rowsWithTrucks)
    {
        _blocksSpace.Prepare(rowsWithBlocks);
        _trucksSpace.Prepare(rowsWithTrucks);

        //_blocksSpace.BlocksEnded += OnLevelCompleted;
        //_ammunitionKits.KitsEnded += OnLevelFailed;
    }

    public void Start()
    {
        _blocksSpace.Start();
        _trucksSpace.Start();
    }

    public void Update(float deltaTime)
    {
        _blocksSpace.Update(deltaTime);
        _trucksSpace.Update(deltaTime);
    }

    public void Exit()
    {
        //_blocksSpace.BlocksEnded -= OnLevelCompleted;
        //_ammunitionKits.KitsEnded -= OnLevelFailed;

        _blocksSpace.Exit();
        _trucksSpace.Exit();
    }

    private void OnLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
    }
}