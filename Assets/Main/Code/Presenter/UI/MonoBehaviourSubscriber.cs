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

        _subscriber = new Subscriber(new SubscriptionUnsubscriptionPair(Subscribe, Unsubscribe));

        _subscriber.Subscribe();
    }

    private void OnEnable()
    {
        _subscriber?.Subscribe();
    }

    private void OnDisable()
    {
        _subscriber?.Unsubscribe();
    }

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    protected abstract void Subscribe();

    protected abstract void Unsubscribe();
}