using System;

public class PlayingState : GameState
{
    private readonly EventBus _eventBus;

    private readonly SphereCastPresenterDetector _presenterDetector;

    private readonly PlayingInputHandler _inputHandler;

    private GameWorld _gameWorld;

    public PlayingState(EventBus eventBus,
                        SphereCastPresenterDetector presenterDetector,
                        PlayingInputHandler playerInput)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _presenterDetector = presenterDetector ?? throw new ArgumentNullException(nameof(presenterDetector));

        _inputHandler = playerInput ?? throw new ArgumentNullException(nameof(playerInput));

        _eventBus.Subscribe<CreatedSignal<GameWorld>>(SetGameWorld);

        _eventBus.Subscribe<ClearedSignal<Game>>(Finish);
    }

    //public event Action LevelReady;
    public event Action PauseRequested;

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

    private void DestroyGameWorld()
    {
        _gameWorld?.Disable();
        _gameWorld?.Clear();

        _gameWorld = null;
    }

    private void Finish(ClearedSignal<Game> _)
    {
        _eventBus.Unsubscribe<ClearedSignal<Game>>(Finish);

        _eventBus.Unsubscribe<CreatedSignal<GameWorld>>(SetGameWorld);
    }

    private void OnInteractPressed()
    {
        if (_presenterDetector.TryGetPresenter(out Presenter presenter) == false)
        {
            return;
        }

        _eventBus.Invoke(new SelectedSignal(presenter.Model));
    }

    private void OnPausePressed()
    {
        PauseRequested?.Invoke();
    }
}