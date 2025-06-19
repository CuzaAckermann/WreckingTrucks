using System;

public class BackgroundGame
{
    private readonly AIClicker _AIClicker;
    private GameWorld _gameWorld;

    public BackgroundGame(AIClicker aiClicker)
    {
        _AIClicker = aiClicker ?? throw new ArgumentNullException(nameof(aiClicker));
    }

    public void Clear()
    {
        _gameWorld?.Clear();
    }

    public void Prepare(GameWorld gameWorld)
    {
        _gameWorld = gameWorld ?? throw new ArgumentNullException(nameof(gameWorld));
        _AIClicker.Prepare(_gameWorld);
    }

    public void Start()
    {
        _gameWorld.Start();
        _AIClicker.Start();
    }

    public void Update(float deltaTime)
    {
        _gameWorld.Update(deltaTime);
        _AIClicker.Update(deltaTime);
    }

    public void Stop()
    {
        _gameWorld.Stop();
        _AIClicker.Stop();
    }
}