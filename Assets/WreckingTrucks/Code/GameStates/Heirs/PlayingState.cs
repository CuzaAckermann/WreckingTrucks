using System;

public class PlayingState : GameState
{
    private readonly PlayingInputHandler _inputHandler;
    private readonly IPresenterDetector<TruckPresenter> _truckPresenterDetector;
    private GameWorld _gameWorld;

    public event Action LevelPassed;
    public event Action PauseRequested;

    public PlayingState(PlayingInputHandler playerInput,
                        IPresenterDetector<TruckPresenter> truckPresenterDetector)
    {
        _inputHandler = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
        _truckPresenterDetector = truckPresenterDetector ?? throw new ArgumentNullException(nameof(truckPresenterDetector));
    }

    public void Clear()
    {
        _gameWorld?.Clear();
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

        _gameWorld.LevelCompleted += OnLevelCompleted;
        _gameWorld.Start();
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
        _gameWorld.Update(deltaTime);
    }

    public override void Exit()
    {
        _gameWorld.Stop();
        _gameWorld.LevelCompleted += OnLevelCompleted;

        _inputHandler.InteractPressed -= OnInteractPressed;
        _inputHandler.PausePressed -= OnPausePressed;

        base.Exit();
    }

    private void OnLevelCompleted()
    {
        LevelPassed?.Invoke();
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
    }

    private void OnPausePressed()
    {
        PauseRequested?.Invoke();
    }
}