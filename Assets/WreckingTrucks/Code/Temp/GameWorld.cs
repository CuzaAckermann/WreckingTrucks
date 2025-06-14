using System;

public class GameWorld
{
    private Space _blocksSpace;
    private Space _trucksSpace;
    //private BulletSpace _bulletSpace;
    //private RoadSpace _roadSpace;

    public GameWorld(Space blocksSpace,
                     Space trucksSpace)
                     //BulletSpace bulletSpace,
                     //RoadSpace roadSpace)
    {
        _blocksSpace = blocksSpace ?? throw new ArgumentNullException(nameof(blocksSpace));
        _trucksSpace = trucksSpace ?? throw new ArgumentNullException(nameof(trucksSpace));
        //_bulletSpace = bulletSpace ?? throw new ArgumentNullException(nameof(bulletSpace));
        //_roadSpace = roadSpace ?? throw new ArgumentNullException(nameof(roadSpace));
    }

    public event Action LevelCompleted;
    public event Action LevelFailed;

    public void Clear()
    {
        _blocksSpace.Clear();
        _trucksSpace.Clear();
        //_roadSpace.Clear();
        //_bulletSpace.Clear();
    }

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
        //_roadSpace.Start();
        //_bulletSpace.Start();
    }

    public void Update(float deltaTime)
    {
        _blocksSpace.Update(deltaTime);
        _trucksSpace.Update(deltaTime);
        //_roadSpace.Update(deltaTime);
    }

    public void Stop()
    {
        //_blocksSpace.BlocksEnded -= OnLevelCompleted;

        _blocksSpace.Exit();
        _trucksSpace.Exit();
        //_roadSpace.Exit();
        //_bulletSpace.Exit();
    }

    public void AddTruckOnRoad(Truck truck)
    {
        //_roadSpace.AddTruck(truck);
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