using System;

public class GameWorld
{
    private readonly EventBus _eventBus;

    private readonly Dispencer _cartrigeBoxDispencer;

    private readonly GameWorldFinalizer _gameWorldFinalizer;

    public GameWorld(Dispencer cartrigeBoxDispencer,
                     EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _cartrigeBoxDispencer = cartrigeBoxDispencer ?? throw new ArgumentNullException(nameof(cartrigeBoxDispencer));

        _gameWorldFinalizer = new GameWorldFinalizer(_eventBus, _cartrigeBoxDispencer);
    }

    public void Clear()
    {
        _eventBus.Unsubscribe<BlockFieldWastedSignal>(Complete);
        _eventBus.Unsubscribe<CartrigeBoxFieldWastedSignal>(WaitLastChanse);

        // отписка от поля с комплектами и отписка от счетчика, если выход был преждевременный
        _gameWorldFinalizer.Disable();
        UnsubscribeFromLevelContinuedSignal();

        _eventBus.Invoke(new ClearedSignal<GameWorld>());
    }

    public void Enable()
    {
        _eventBus.Subscribe<BlockFieldWastedSignal>(Complete);
        _eventBus.Subscribe<CartrigeBoxFieldWastedSignal>(WaitLastChanse);

        _eventBus.Invoke(new EnabledSignal<GameWorld>());
    }

    public void Disable()
    {
        _eventBus.Unsubscribe<BlockFieldWastedSignal>(Complete);
        _eventBus.Unsubscribe<CartrigeBoxFieldWastedSignal>(WaitLastChanse);

        _eventBus.Invoke(new DisabledSignal<GameWorld>());
    }

    private void Complete(BlockFieldWastedSignal _)
    {
        _eventBus.Invoke(new CompletedSignal<GameWorld>());

        _gameWorldFinalizer.Disable();

        UnsubscribeFromLevelContinuedSignal();
    }

    private void WaitLastChanse(CartrigeBoxFieldWastedSignal _)
    {
        _eventBus.Subscribe<ContinuedSignal<GameWorld>>(Continue);

        _gameWorldFinalizer.Enable();
    }

    private void UnsubscribeFromLevelContinuedSignal()
    {
        _eventBus.Unsubscribe<ContinuedSignal<GameWorld>>(Continue);
    }

    private void Continue(ContinuedSignal<GameWorld> _)
    {
        _eventBus.Unsubscribe<ContinuedSignal<GameWorld>>(Continue);
    }
}