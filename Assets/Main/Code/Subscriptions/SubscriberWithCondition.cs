using System;

public class SubscriberWithCondition
{
    private readonly Func<bool> _subscription;
    private readonly Func<bool> _unsubscription;

    private bool _isSubscribed;

    public SubscriberWithCondition(Func<bool> subscription,
                                   Func<bool> unsubscription)
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

        if (_subscription() == false)
        {
            return;
        }

        _isSubscribed = true;
    }

    public void Unsubscribe()
    {
        if (_isSubscribed == false)
        {
            return;
        }

        if (_unsubscription() == false)
        {
            return;
        }

        _isSubscribed = false;
    }
}