using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowAnimation : ITickable
{
    private readonly CanvasGroup _canvasGroup;
    private readonly float _animationSpeed;

    private float _targetAlpha;

    public WindowAnimation(CanvasGroup canvasGroup, float animationSpeed)
    {
        _canvasGroup = canvasGroup ? canvasGroup : throw new ArgumentNullException(nameof(canvasGroup));
        _animationSpeed = animationSpeed > 0 ? animationSpeed : throw new ArgumentNullException(nameof(animationSpeed));

        _targetAlpha = 0;
    }

    public event Action<ITickable> Activated;

    public event Action<ITickable> Deactivated;

    public event Action<IDestroyable> Destroyed;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }

    public void StartShow()
    {
        _canvasGroup.interactable = true;

        _targetAlpha = 1;

        Activated?.Invoke(this);
    }

    public void StartHide()
    {
        _canvasGroup.blocksRaycasts = false;

        _targetAlpha = 0;

        Activated?.Invoke(this);
    }

    public void Tick(float deltaTime)
    {
        _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, _targetAlpha, _animationSpeed * deltaTime);

        if (_canvasGroup.alpha == _targetAlpha)
        {
            if (_targetAlpha == 0)
            {
                FinishHide();
            }
            else if (_targetAlpha == 1)
            {
                FinishShow();
            }

            Deactivated?.Invoke(this);
        }
    }

    private void FinishShow()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    private void FinishHide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
    }
}