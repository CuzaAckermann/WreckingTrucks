using System;

public class GameWorld
{
    private readonly Space _blocksSpace;
    private readonly TruckSpace _trucksSpace;
    private readonly RoadSpace _roadSpace;

    public GameWorld(Space blocksSpace,
                     TruckSpace trucksSpace,
                     RoadSpace roadSpace)
    {
        _blocksSpace = blocksSpace ?? throw new ArgumentNullException(nameof(blocksSpace));
        _trucksSpace = trucksSpace ?? throw new ArgumentNullException(nameof(trucksSpace));
        _roadSpace = roadSpace ?? throw new ArgumentNullException(nameof(roadSpace));
    }

    public event Action LevelCompleted;
    public event Action LevelFailed;

    public TruckField TruckField => _trucksSpace.TruckField;

    public void Clear()
    {
        _blocksSpace.Clear();
        _trucksSpace.Clear();
        _roadSpace.Clear();
    }

    public void Prepare(LevelSettings levelSettings)
    {
        _blocksSpace.Prepare(levelSettings.BlocksSpaceSettings);
        _trucksSpace.Prepare(levelSettings.TrucksSpaceSettings);
        _roadSpace.Prepare();

        //_blocksSpace.BlocksEnded += OnLevelCompleted;
    }

    public void Start()
    {
        _blocksSpace.Start();
        _trucksSpace.Start();
        _roadSpace.Start();
    }

    public void Update(float deltaTime)
    {
        _blocksSpace.Update(deltaTime);
        _trucksSpace.Update(deltaTime);
        _roadSpace.Update(deltaTime);
    }

    public void Stop()
    {
        //_blocksSpace.BlocksEnded -= OnLevelCompleted;

        _blocksSpace.Stop();
        _trucksSpace.Stop();
        _roadSpace.Stop();
    }

    public void AddTruckOnRoad(Truck truck)
    {
        if (_trucksSpace.TryRemoveTruck(truck))
        {
            _roadSpace.AddTruck(truck);
        }
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