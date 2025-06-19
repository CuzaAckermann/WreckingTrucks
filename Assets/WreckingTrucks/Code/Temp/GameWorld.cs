using System;
using System.Collections.Generic;

public class GameWorld
{
    private readonly Space _blocksSpace;
    private readonly TruckSpace _trucksSpace;
    private readonly RoadSpace _roadSpace;
    private readonly ShootingSpace _shootingSpace;

    private ModelsSelector _blocksSelector;

    public GameWorld(Space blocksSpace,
                     TruckSpace trucksSpace,
                     RoadSpace roadSpace,
                     ShootingSpace shootingSpace)
    {
        _blocksSpace = blocksSpace ?? throw new ArgumentNullException(nameof(blocksSpace));
        _trucksSpace = trucksSpace ?? throw new ArgumentNullException(nameof(trucksSpace));
        _roadSpace = roadSpace ?? throw new ArgumentNullException(nameof(roadSpace));
        _shootingSpace = shootingSpace ?? throw new ArgumentNullException(nameof(shootingSpace));

        _blocksSelector = new ModelsSelector(_blocksSpace.Field);
    }

    public event Action LevelCompleted;
    public event Action LevelFailed;

    public TruckField TruckField => _trucksSpace.TruckField;

    public void Clear()
    {
        _blocksSpace.Clear();
        _trucksSpace.Clear();
        _roadSpace.Clear();
        _shootingSpace.Clear();
    }

    public void Prepare(LevelSettings levelSettings)
    {
        _blocksSpace.Prepare(levelSettings.BlocksSpaceSettings);
        _trucksSpace.Prepare(levelSettings.TrucksSpaceSettings);
        _roadSpace.Prepare();
        _shootingSpace.Prepare();

        //_blocksSpace.BlocksEnded += OnLevelCompleted;
    }

    public void Start()
    {
        _blocksSpace.Start();
        _trucksSpace.Start();
        _roadSpace.Start();
        _shootingSpace.Start();
    }

    public void Update(float deltaTime)
    {
        _blocksSpace.Update(deltaTime);
        _trucksSpace.Update(deltaTime);
        _roadSpace.Update(deltaTime);
        _shootingSpace.Update(deltaTime);
    }

    public void Stop()
    {
        //_blocksSpace.BlocksEnded -= OnLevelCompleted;

        _blocksSpace.Stop();
        _trucksSpace.Stop();
        _roadSpace.Stop();
        _shootingSpace.Stop();
    }

    public void AddTruckOnRoad(Truck truck)
    {
        if (_trucksSpace.TryRemoveTruck(truck))
        {
            List<Model> destroyableModels = _blocksSelector.GetBlocksOneType(truck.DestroyableType);
            truck.SetDestroyableModels(destroyableModels);
            _roadSpace.AddTruck(truck);
            _shootingSpace.AddGun(truck.Gun);
            truck.StartShoot();
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