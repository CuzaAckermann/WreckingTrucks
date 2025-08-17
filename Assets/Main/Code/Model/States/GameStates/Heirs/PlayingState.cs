using System;

public class PlayingState : GameState
{
    private readonly IPresenterDetector<TruckPresenter> _truckPresenterDetector;
    private readonly IPresenterDetector<PlanePresenter> _planePresenterDetector;
    private readonly PlayingInputHandler _inputHandler;

    private GameWorld _gameWorld;

    public PlayingState(IPresenterDetector<TruckPresenter> truckPresenterDetector,
                        IPresenterDetector<PlanePresenter> planePresenterDetector,
                        PlayingInputHandler playerInput)
    {
        _truckPresenterDetector = truckPresenterDetector ?? throw new ArgumentNullException(nameof(truckPresenterDetector));
        _planePresenterDetector = planePresenterDetector ?? throw new ArgumentNullException(nameof(planePresenterDetector));
        _inputHandler = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
    }

    public event Action LevelReady;
    public event Action PauseRequested;

    public event Action LevelPassed;
    public event Action LevelFailed;

    public GameWorld GameWorld => _gameWorld;

    public void Clear()
    {
        _gameWorld?.Destroy();
    }

    public void Prepare(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
    }

    public override void Enter()
    {
        base.Enter();

        _inputHandler.InteractPressed += OnInteractPressed;
        _inputHandler.PausePressed += OnPausePressed;

        _gameWorld.LevelPassed += OnLevelPassed;
        _gameWorld.LevelFailed += OnLevelFailed;
    }

    public void EnableGameWorld()
    {
        _gameWorld.Enable();
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
    }

    public void DisableGameWorld()
    {
        _gameWorld?.Disable();
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
                _gameWorld.AddTruckOnRoad(truck);
            }
        }
        else if (_planePresenterDetector.TryGetPresenter(out PlanePresenter planePresenter))
        {
            Logger.Log("Prok");
        }
    }

    private void OnPausePressed()
    {
        PauseRequested?.Invoke();
    }
}