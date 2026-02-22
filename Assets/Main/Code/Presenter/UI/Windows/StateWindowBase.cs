using System;
using UnityEngine;

public abstract class StateWindowBase : MonoBehaviour, IAnimatable
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public abstract Type BoundStateType { get; }

    public float Value
    {
        get
        {
            return _canvasGroup.alpha;
        }
        set
        {
            Validator.ValidateMin(value, 0, false);
            Validator.ValidateMax(value, 1, false);

            if (value == _canvasGroup.alpha)
            {
                return;
            }

            _canvasGroup.alpha = value;
        }
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
}