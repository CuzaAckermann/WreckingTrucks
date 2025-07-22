using System;

public class BackgroundGame
{
    private readonly ComputerPlayer _computerPlayer;
    
    private GameWorld _gameWorld;
    private bool _isEnable;

    public BackgroundGame(ComputerPlayer computerPlayer)
    {
        _computerPlayer = computerPlayer ?? throw new ArgumentNullException(nameof(computerPlayer));
        _isEnable = false;
    }

    public void Clear()
    {
        _gameWorld?.Destroy();
    }

    public void Prepare(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
        _computerPlayer.Prepare(_gameWorld);
    }

    public void Enable()
    {
        if (_isEnable == false)
        {
            _gameWorld.Enable();
            _computerPlayer.Enable();
            _isEnable = true;
        }

    }

    public void Disable()
    {
        if (_isEnable)
        {
            _gameWorld.Disable();
            _computerPlayer.Disable();
            _isEnable = false;
        }
    }
}