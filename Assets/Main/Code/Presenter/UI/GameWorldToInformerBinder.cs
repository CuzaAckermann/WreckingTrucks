using System;
using UnityEngine;

public class GameWorldToInformerBinder : MonoBehaviour
{
    [SerializeField] private GameWorldInformer _gameWorldInformer;

    private Game _game;
    private bool _isSubscribed;

    public void Initialize(Game game)
    {
        if (_game != null)
        {
            UnsubscribeFromGame();
        }

        _game = game ?? throw new ArgumentNullException(nameof(game));
        SubscribeToGame();
    }

    private void OnEnable()
    {
        if (_game != null && _isSubscribed == false)
        {
            SubscribeToGame();
            _isSubscribed = true;
        }
    }

    private void OnDisable()
    {
        if (_game != null && _isSubscribed)
        {
            UnsubscribeFromGame();
            _isSubscribed = false;
        }
    }

    private void SubscribeToGame()
    {
        _game.GameWorldCreated += OnGameWorldCreated;
        _game.GameWorldDestroyed += OnGameWorldDestroyed;
    }

    private void UnsubscribeFromGame()
    {
        _game.GameWorldCreated -= OnGameWorldCreated;
        _game.GameWorldDestroyed -= OnGameWorldDestroyed;
    }

    private void OnGameWorldCreated(GameWorld gameWorld)
    {
        _gameWorldInformer.Initialize(gameWorld.CartrigeBoxSpace);
        _gameWorldInformer.Show();
    }

    private void OnGameWorldDestroyed()
    {
        _gameWorldInformer.Hide();
    }
}