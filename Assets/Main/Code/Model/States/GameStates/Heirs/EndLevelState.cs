using System;

public class EndLevelState : GameState
{
    private readonly EndLevelProcess _endLevelProcess;

    private GameWorld _gameWorld;

    public EndLevelState(EndLevelProcess endLevelProcess)
    {
        _endLevelProcess = endLevelProcess ?? throw new ArgumentNullException(nameof(endLevelProcess));
    }

    public override void Enter()
    {
        base.Enter();

        _endLevelProcess.SetCartrigeBoxSpace(_gameWorld.CartrigeBoxSpace);
        _endLevelProcess.Enable();
    }

    public override void Exit()
    {
        _endLevelProcess.Disable();
        _endLevelProcess.Clear();
        base.Exit();
    }

    public void SetGameWorld(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
    }
}