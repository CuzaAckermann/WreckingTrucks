using System;

public class PlayingState : GameState
{
    private readonly IPresenterDetector<TruckPresenter> _truckPresenterDetector;
    private readonly IPresenterDetector<BlockPresenter> _blockPresenterDetector;
    private readonly IPresenterDetector<PlanePresenter> _planePresenterDetector;
    private readonly PlayingInputHandler _inputHandler;

    private GameWorld _gameWorld;

    public PlayingState(IPresenterDetector<TruckPresenter> truckPresenterDetector,
                        IPresenterDetector<BlockPresenter> blockPresenterDetector,
                        IPresenterDetector<PlanePresenter> planePresenterDetector,
                        PlayingInputHandler playerInput)
    {
        _truckPresenterDetector = truckPresenterDetector ?? throw new ArgumentNullException(nameof(truckPresenterDetector));
        _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
        _planePresenterDetector = planePresenterDetector ?? throw new ArgumentNullException(nameof(planePresenterDetector));
        _inputHandler = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
    }

    //public event Action LevelReady;
    public event Action PauseRequested;

    public event Action LevelPassed;
    public event Action LevelFailed;

    public void Clear()
    {
        _gameWorld?.Destroy();
    }

    public void Prepare(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
    }

    public void EnableGameWorld()
    {
        _gameWorld.Enable();
    }

    public void DisableGameWorld()
    {
        _gameWorld?.Disable();
    }

    public override void Enter()
    {
        base.Enter();

        _inputHandler.InteractPressed += OnInteractPressed;
        _inputHandler.PausePressed += OnPausePressed;

        _gameWorld.LevelPassed += OnLevelPassed;
        _gameWorld.LevelFailed += OnLevelFailed;
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
    }

    public override void Exit()
    {
        _gameWorld.LevelPassed -= OnLevelPassed;
        _gameWorld.LevelFailed -= OnLevelFailed;

        _inputHandler.InteractPressed -= OnInteractPressed;
        _inputHandler.PausePressed -= OnPausePressed;

        base.Exit();
    }

    private void OnLevelPassed()
    {
        LevelPassed?.Invoke();
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
    }

    private void OnInteractPressed()
    {
        if (_truckPresenterDetector.TryGetPresenter(out TruckPresenter truckPresenter))
        {
            if (truckPresenter.Model is Truck truck)
            {
                _gameWorld.ReleaseTruck(truck);
            }
        }
        else if (_blockPresenterDetector.TryGetPresenter(out BlockPresenter blockPresenter))
        {
            if (blockPresenter.Model is Block block)
            {
                block.Destroy();
            }
        }
        else if (_planePresenterDetector.TryGetPresenter(out PlanePresenter planePresenter))
        {
            if (planePresenter.Model is Plane plane)
            {
                _gameWorld.ReleasePlane(plane);
            }
        }
    }

    private void OnPausePressed()
    {
        PauseRequested?.Invoke();
    }
}