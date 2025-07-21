using System;

public class PlayingState : GameState
{
    private readonly IPresenterDetector<TruckPresenter> _truckPresenterDetector;
    private readonly PlayingInputHandler _inputHandler;

    private GameWorld _gameWorld;

    public PlayingState(IPresenterDetector<TruckPresenter> truckPresenterDetector,
                        PlayingInputHandler playerInput)
    {
        _truckPresenterDetector = truckPresenterDetector ?? throw new ArgumentNullException(nameof(truckPresenterDetector));
        _inputHandler = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
    }

    public event Action LevelPassed;
    public event Action LevelFailed;

    public event Action PauseRequested;

    // ÑÎÌÍÈÒÅËÜÍÎ
    //public GameWorld GameWorld => _gameWorld;

    public void Clear()
    {
        _gameWorld?.Clear();
    }

    public void Prepare(GameWorld gameWorld,
                        FillingCard blockFillingCard,
                        FillingCard truckFillingCard,
                        FillingCard cartrigeBoxFillingCard)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
        _gameWorld.Prepare(blockFillingCard, truckFillingCard, cartrigeBoxFillingCard);
    }

    public override void Enter()
    {
        base.Enter();

        _inputHandler.InteractPressed += OnInteractPressed;
        _inputHandler.PausePressed += OnPausePressed;

        _gameWorld.LevelCompleted += OnLevelCompleted;
        _gameWorld.LevelFailed += OnLevelFailed;

        _gameWorld.Enable();
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
    }

    public override void Exit()
    {
        _gameWorld.Disable();

        _gameWorld.LevelCompleted -= OnLevelCompleted;
        _gameWorld.LevelFailed -= OnLevelFailed;

        _inputHandler.InteractPressed -= OnInteractPressed;
        _inputHandler.PausePressed -= OnPausePressed;

        base.Exit();
    }

    private void OnLevelCompleted()
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
    }

    private void OnPausePressed()
    {
        PauseRequested?.Invoke();
    }
}