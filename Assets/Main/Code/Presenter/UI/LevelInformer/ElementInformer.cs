using System;
using UnityEngine;

public abstract class ElementInformer : MonoBehaviour
{
    private EventBus _eventBus;

    private bool _isInited;
    private bool _needShowed;

    public virtual void Init(EventBus eventBus, float height)
    {
        if (_isInited)
        {
            return;
        }

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        PrepareForInit(height);
        Subscribe();
        Hide();

        _isInited = true;
        _needShowed = false;
    }

    private void OnEnable()
    {
        if (_isInited == false)
        {
            return;
        }

        Subscribe();

        if (_needShowed == false)
        {
            return;
        }

        Show();
    }

    private void OnDisable()
    {
        if (_isInited == false)
        {
            return;
        }

        Unsubscribe();

        Hide();
    }

    protected abstract void PrepareForInit(float height);

    protected abstract void PrepareForShowing(CreatedSignal<Level> levelCreatedSignal);

    protected abstract void Show();

    protected abstract void Hide();

    protected void Subscribe()
    {
        _eventBus.Subscribe<CreatedSignal<Level>>(Show);
        _eventBus.Subscribe<ClearedSignal<Level>>(Hide);
    }

    protected void Unsubscribe()
    {
        _eventBus.Unsubscribe<CreatedSignal<Level>>(Show);
        _eventBus.Unsubscribe<ClearedSignal<Level>>(Hide);
    }

    private void Show(CreatedSignal<Level> levelCreatedSignal)
    {
        PrepareForShowing(levelCreatedSignal);

        _needShowed = true;
        Show();
    }

    private void Hide(ClearedSignal<Level> levelClearedSignal)
    {
        _needShowed = false;
        Hide();
    }
}