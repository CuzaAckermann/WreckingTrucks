using System;
using UnityEngine;

public abstract class WindowOfState<GS> : MonoBehaviour where GS : InputState
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private GS _gameState;

    private bool _isSubscribed;

    public virtual void Init(GS gameState)
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

    public virtual void Show()
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

    protected virtual void Subscribe()
    {
        if (_gameState != null && _isSubscribed == false)
        {
            _gameState.Entered += Show;
            _gameState.Exited += Hide;

            _isSubscribed = true;
        }
    }

    protected virtual void Unsubscribe()
    {
        if (_gameState != null && _isSubscribed)
        {
            _gameState.Entered -= Show;
            _gameState.Exited -= Hide;
            
            _isSubscribed = false;
        }
    }
}