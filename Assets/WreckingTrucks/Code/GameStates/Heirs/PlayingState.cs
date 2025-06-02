using System;

public class PlayingState : GameState
{
    private LevelStarter _levelStarter;
    private BlocksField _blocksField;

    private Stopwatch _stopwatch;

    public event Action LevelPrepared;
    public event Action LevelPassed;
    public event Action LevelFailed;

    public event Action IntervalPassed;

    public PlayingState(IWindow window, LevelStarter levelStarter) : base(window)
    {
        //_levelStarter = levelStarter ?? throw new ArgumentNullException(nameof(levelStarter));
        _stopwatch = new Stopwatch(0.5f);
        _stopwatch.IntervalPassed += OnIntervalPassed;
        _stopwatch.Start();
    }

    public override void Enter()
    {
        //_blocksField.AllColumnIsEmpty += OnLevelPassed;

        //_levelStarter.PrepareLevel();
        //_levelStarter.StartLevel();

        LevelPrepared?.Invoke();

        base.Enter();
    }

    public override void Update(float deltaTime)
    {
        _stopwatch.Tick(deltaTime);
    }

    public override void Exit()
    {
        //_blocksField.AllColumnIsEmpty -= OnLevelPassed;

        base.Exit();
    }

    public void ResetLevel()
    {

    }

    private void OnLevelPassed()
    {
        LevelPassed?.Invoke();
    }

    private void OnLevelFailed()
    {
        LevelFailed?.Invoke();
    }

    private void OnIntervalPassed()
    {
        IntervalPassed?.Invoke();
    }
}