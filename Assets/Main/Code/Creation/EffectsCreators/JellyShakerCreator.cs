using System;

public class JellyShakerCreator : ITickableCreator
{
    private readonly EventBus _eventBus;
    private readonly PresenterProduction _presenterProduction;

    public JellyShakerCreator(EventBus eventBus, PresenterProduction presenterProduction)
    {
        Validator.ValidateNotNull(eventBus, presenterProduction);

        _eventBus = eventBus;
        _presenterProduction = presenterProduction;
    }

    public event Action<ITickable> TickableCreated;

    public JellyShaker Create()
    {
        JellyShaker shaker = new JellyShaker(_eventBus, _presenterProduction, 100);

        TickableCreated?.Invoke(shaker);

        return shaker;
    }
}