using System;
using UnityEngine;

public abstract class WindowOfState<GS> : MonoBehaviour, ITickableCreator where GS : InputState
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private GS _gameState;

    private WindowAnimation _windowAnimation;

    private bool _isSubscribed;

    public virtual void Init(GS gameState, float animationSpeed)
    {
        Unsubscribe();

        _gameState = gameState ?? throw new ArgumentNullException(nameof(gameState));

        _windowAnimation = new WindowAnimation(_canvasGroup, animationSpeed);
        TickableCreated?.Invoke(_windowAnimation);

        Subscribe();
    }

    public event Action<ITickable> TickableCreated;

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
        _windowAnimation.StartShow();

        //_canvasGroup.alpha = 1;
        //_canvasGroup.blocksRaycasts = true;
        //_canvasGroup.interactable = true;
    }

    public void Hide()
    {
        _windowAnimation.StartHide();

        //_canvasGroup.alpha = 0;
        //_canvasGroup.blocksRaycasts = false;
        //_canvasGroup.interactable = false;
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

    protected void OnTickableCreated(ITickable tickable)
    {
        TickableCreated?.Invoke(tickable);
    }
}