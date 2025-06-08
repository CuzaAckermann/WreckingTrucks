using System;

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

    public void Prepare(FillingCard<Type> fillingCardWithBlocks,
                        FillingCard<Type> fillingCardWithTrucks)
    {
        _blocksSpace.Prepare(fillingCardWithBlocks);
        _trucksSpace.Prepare(fillingCardWithTrucks);

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