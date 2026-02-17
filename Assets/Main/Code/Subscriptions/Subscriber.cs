using System;

public class Subscriber
{
    private readonly Action _subscription;
    private readonly Action _unsubscription;

    private bool _isSubscribed;

    public Subscriber(Action subscription,
                      Action unsubscription)
    {
        Validator.ValidateNotNull(subscription, unsubscription);

        _subscription = subscription;
        _unsubscription = unsubscription;

        _isSubscribed = false;
    }

    public void Subscribe()
    {
        if (_isSubscribed)
        {
            return;
        }

        _subscription();

        _isSubscribed = true;
    }

    public void Unsubscribe()
    {
        if (_isSubscribed == false)
        {
            return;
        }

        _unsubscription();

        _isSubscribed = false;
    }
}