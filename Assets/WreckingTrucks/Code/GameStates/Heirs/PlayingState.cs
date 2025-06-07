using System;

public class PlayingState : GameState
{
    private PlayingInputHandler _inputHandler;
    private IPresenterDetector<BlockPresenter> _blockPresenterDetector;

    private GameWorld _gameWorld;
    private bool _isPaused;

    public event Action LevelPassed;
    public event Action LevelFailed;
    public event Action PauseRequested;

    public PlayingState(PlayingInputHandler playerInput,
                        IPresenterDetector<BlockPresenter> blockPresenterDetector)
    {
        _inputHandler = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
        _blockPresenterDetector = blockPresenterDetector ?? throw new ArgumentNullException(nameof(blockPresenterDetector));
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

        _gameWorld.Start();
    }

    public override void Update(float deltaTime)
    {
        _inputHandler.Update();
        _gameWorld.Update(deltaTime);
    }

    public override void Exit()
    {
        _gameWorld.Exit();

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
        if (_blockPresenterDetector.TryGetPresenter(out BlockPresenter blockPresenter))
        {
            blockPresenter.Destroy();
        }
    }

    private void OnPausePressed()
    {
        PauseRequested?.Invoke();
    }
}