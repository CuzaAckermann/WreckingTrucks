using System;

public class SubscriptionUnsubscriptionPair
{
    public SubscriptionUnsubscriptionPair(Action subscription,
                                          Action unsubscription)
    {
        Validator.ValidateNotNull(subscription, unsubscription);

        Subscription = subscription;
        Unsubscription = unsubscription;

        IsSubscribed = false;
    }

    public Action Subscription { get; private set; }

    public Action Unsubscription { get; private set; }

    public bool IsSubscribed { get; private set; }

    public void Switch()
    {
        IsSubscribed = IsSubscribed == false;
    }
}