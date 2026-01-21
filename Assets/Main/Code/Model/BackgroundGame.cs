using System;

public class BackgroundGame
{
    private readonly ComputerPlayer _computerPlayer;
    
    private Level _gameWorld;
    private bool _isEnable;

    public BackgroundGame(ComputerPlayer computerPlayer)
    {
        _computerPlayer = computerPlayer ?? throw new ArgumentNullException(nameof(computerPlayer));
        _isEnable = false;
    }

    public void Clear()
    {
        _gameWorld?.Clear();
    }

    public void Prepare(Level gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
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