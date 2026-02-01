using System;

public class JellyShakerCreator : ITickableCreator
{
    public event Action<ITickable> TickableCreated;

    public JellyShaker Create(EventBus eventBus)
    {
        JellyShaker shaker = new JellyShaker(100, eventBus);

        TickableCreated?.Invoke(shaker);

        return shaker;
    }
}