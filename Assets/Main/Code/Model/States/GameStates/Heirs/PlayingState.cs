using System;

public class PlayingState : GameState
{
    private readonly EventBus _eventBus;

    private readonly IPresenterDetector<TruckPresenter> _truckPresenterDetector;
    private readonly IPresenterDetector<BlockPresenter> _blockPresenterDetector;
    private readonly IPresenterDetector<PlanePresenter> _planePresenterDetector;
    private readonly PlayingInputHandler _inputHandler;

    private GameWorld _gameWorld;

    public PlayingState(EventBus eventBus,
                        IPresenterDetector<TruckPresenter> truckPresenterDetector,
                        IPresenterDetector<BlockPresenter> blockPresenterDetector,
                        IPresenterDetector<PlanePresenter> planePresenterDetector,
                        PlayingInputHandler playerInput)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _truckPresenterDetector = truckPresenterDetector ?? throw new ArgumentNullException(nameof(truckPresenterDetector));
        _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
        _planePresenterDetector = planePresenterDetector ?? throw new ArgumentNullException(nameof(planePresenterDetector));
        _inputHandler = playerInput ?? throw new ArgumentNullException(nameof(playerInput));

        _eventBus.Subscribe<CreatedSignal<GameWorld>>(SetGameWorld);

        _eventBus.Subscribe<GameClearedSignal>(Finish);
    }

    //public event Action LevelReady;
    public event Action PauseRequested;

    public void DestroyGameWorld()
    {
        _gameWorld?.Disable();
        _gameWorld?.Destroy();

        _gameWorld = null;
    }

    public override void Enter()
    {
        base.Enter();

        _gameWorld.Enable();

        _inputHandler.InteractPressed += OnInteractPressed;
        _inputHandler.PausePressed += OnPausePressed;
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
    }

    public override void Exit()
    {
        _inputHandler.InteractPressed -= OnInteractPressed;
        _inputHandler.PausePressed -= OnPausePressed;

        base.Exit();
    }

    private void SetGameWorld(CreatedSignal<GameWorld> createdSignal)
    {
        DestroyGameWorld();

        _gameWorld = createdSignal.Creatable;

        // Prepare new _gameWorld
    }

    private void Finish(GameClearedSignal _)
    {
        _eventBus.Unsubscribe<GameClearedSignal>(Finish);

        _eventBus.Unsubscribe<CreatedSignal<GameWorld>>(SetGameWorld);
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