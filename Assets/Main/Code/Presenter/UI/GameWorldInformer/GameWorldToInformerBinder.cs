using System;
using UnityEngine;

public class GameWorldToInformerBinder : MonoBehaviour
{
    [SerializeField] private GameWorldInformer _gameWorldInformer;

    private GameWorldCreator _gameWorldCreator;
    private bool _isSubscribed;

    public void Initialize(GameWorldCreator gameWorldCreator)
    {
        if (_gameWorldCreator != null)
        {
            UnsubscribeFromGameWorldCreator();
        }

        _gameWorldCreator = gameWorldCreator ?? throw new ArgumentNullException(nameof(gameWorldCreator));

        _gameWorldInformer.Initialize();

        SubscribeToGameWorldCreator();
    }

    private void OnEnable()
    {
        if (_gameWorldCreator != null && _isSubscribed == false)
        {
            SubscribeToGameWorldCreator();
            _isSubscribed = true;
        }
    }

    private void OnDisable()
    {
        if (_gameWorldCreator != null && _isSubscribed)
        {
            UnsubscribeFromGameWorldCreator();
            _isSubscribed = false;
        }
    }

    private void SubscribeToGameWorldCreator()
    {
        _gameWorldCreator.GameWorldCreated += OnGameWorldCreated;
    }

    private void UnsubscribeFromGameWorldCreator()
    {
        _gameWorldCreator.GameWorldCreated -= OnGameWorldCreated;
    }

    private void OnGameWorldCreated(GameWorld gameWorld)
    {
        _gameWorldInformer.ConnectGameWorld(gameWorld);
    }
}