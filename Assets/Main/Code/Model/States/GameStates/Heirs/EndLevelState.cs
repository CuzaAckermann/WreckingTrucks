using System;

public class EndLevelState : GameState
{
    private readonly EndLevelProcess _endLevelProcess;
    private readonly GameWorldCreator _gameWorldCreator;

    private GameWorld _gameWorld;

    private bool _isSubscribed = false;

    public EndLevelState(GameWorldCreator gameWorldCreator, EndLevelProcess endLevelProcess)
    {
        _gameWorldCreator = gameWorldCreator ?? throw new ArgumentNullException(nameof(gameWorldCreator));
        _endLevelProcess = endLevelProcess ?? throw new ArgumentNullException(nameof(endLevelProcess));

        SubscribeToGameWorldCreator();
    }

    public override void Enter()
    {
        base.Enter();

        _endLevelProcess.SetCartrigeBoxSpace(_gameWorld.CartrigeBoxField);
        _endLevelProcess.Enable();
    }

    public override void Exit()
    {
        _endLevelProcess.Disable();
        _endLevelProcess.Clear();
        base.Exit();
    }

    private void SubscribeToGameWorldCreator()
    {
        if (_isSubscribed == false)
        {
            _gameWorldCreator.GameWorldCreated += SetGameWorld;

            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromGameWorldCreator()
    {
        if (_isSubscribed)
        {
            _gameWorldCreator.GameWorldCreated -= SetGameWorld;

            _isSubscribed = false;
        }
    }

    private void SetGameWorld(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
    }
}