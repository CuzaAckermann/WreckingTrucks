using System;

public class JellyShakerCreator : ITickableCreator
{
    private readonly EventBus _eventBus;

    public JellyShakerCreator(EventBus eventBus)
    {
        Validator.ValidateNotNull(eventBus);

        _eventBus = eventBus;
    }

    public event Action<ITickable> TickableCreated;

    public JellyShaker Create()
    {
        JellyShaker shaker = new JellyShaker(100, _eventBus);

        TickableCreated?.Invoke(shaker);

        return shaker;
    }
}