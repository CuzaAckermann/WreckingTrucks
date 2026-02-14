using System;
using UnityEngine;

public abstract class MonoBehaviourSubscriber<N> : MonoBehaviour
{
    private N _notifier;
    private bool _isSubscribed = false;

    private bool _isInited;
    private bool _isReassignable = false;

    public void Init(N notifier, bool isReassignable = false)
    {
        if (_isInited == false)
        {
            _isReassignable = isReassignable;
        }
        else
        {
            if (_isReassignable == false)
            {
                return;
            }

            Unsubscribe();
        }

        _notifier = notifier ?? throw new ArgumentNullException(nameof(notifier));

        Subscribe();

        _isInited = true;
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    protected abstract void SubscribeToNotifier(N notifier);

    protected abstract void UnsubscribeFromNotifier(N notifier);

    private void Subscribe()
    {
        if (_notifier != null && _isSubscribed == false)
        {
            SubscribeToNotifier(_notifier);

            _isSubscribed = true;
        }
    }

    private void Unsubscribe()
    {
        if (_isSubscribed)
        {
            UnsubscribeFromNotifier(_notifier);

            _isSubscribed = false;
        }
    }
}