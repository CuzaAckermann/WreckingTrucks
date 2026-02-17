using UnityEngine;

public abstract class MonoBehaviourSubscriber : MonoBehaviour
{
    private Subscriber _subscriber;

    public void Init()
    {
        if (_subscriber != null)
        {
            return;
        }

        _subscriber = new Subscriber(Subscribe, Unsubscribe);

        OnEnable();
    }

    private void OnEnable()
    {
        _subscriber?.Subscribe();
    }

    private void OnDisable()
    {
        _subscriber?.Unsubscribe();
    }

    protected abstract void Subscribe();

    protected abstract void Unsubscribe();
}