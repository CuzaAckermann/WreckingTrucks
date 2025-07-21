using System;
using UnityEngine;

public abstract class Window<GS> : MonoBehaviour where GS : GameState
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private GS _gameState;

    private bool _isSubscribed;

    public virtual void Initialize(GS gameState)
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

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    protected abstract void SubscribeToInteractables(GS gameState);

    protected abstract void UnsubscribeFromInteractables(GS gameState);

    private void Subscribe()
    {
        if (_gameState != null && _isSubscribed == false)
        {
            _gameState.Entered += Show;
            _gameState.Exited += Hide;
            SubscribeToInteractables(_gameState);
            _isSubscribed = true;
        }
    }

    private void Unsubscribe()
    {
        if (_gameState != null && _isSubscribed)
        {
            _gameState.Entered -= Show;
            _gameState.Exited -= Hide;
            UnsubscribeFromInteractables(_gameState);
            _isSubscribed = false;
        }
    }
}