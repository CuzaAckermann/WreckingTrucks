public class Subscriber
{
    private readonly SubscriptionUnsubscriptionPair _pair;

    public Subscriber(SubscriptionUnsubscriptionPair pair)
    {
        Validator.ValidateNotNull(pair);

        _pair = pair;
    }

    public void Subscribe()
    {
        if (_pair.IsSubscribed == false)
        {
            _pair.Subscription();

            _pair.Switch();
        }
    }

    public void Unsubscribe()
    {
        if (_pair.IsSubscribed)
        {
            _pair.Unsubscription();

            _pair.Switch();
        }
    }
}