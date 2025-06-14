using System;
using UnityEngine;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private GameState _gameState;
    private bool _isSubscribed;

    public void Initialize(GameState gameState)
    {
        Unsubscribe();

        _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    protected abstract void SubscribeToInteractables();

    protected abstract void UnsubscribeFromInteractables();

    private void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    private void Subscribe()
    {
        if (_gameState != null && _isSubscribed == false)
        {
            _gameState.Entered += Show;
            _gameState.Exited += Hide;
            SubscribeToInteractables();
            _isSubscribed = true;
        }
    }

    private void Unsubscribe()
    {
        if (_gameState != null && _isSubscribed)
        {
            _gameState.Entered -= Show;
            _gameState.Exited -= Hide;
            UnsubscribeFromInteractables();
            _isSubscribed = false;
        }
    }
}