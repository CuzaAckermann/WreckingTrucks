using System;

public class CartrigeBoxFieldFiller
{
    private readonly FillingStrategy<CartrigeBox> _fillingStrategy;

    private readonly FillingState<CartrigeBox> _fillingState;

    private readonly EventBus _eventBus;

    private bool _isFillingCardEmpty;

    public CartrigeBoxFieldFiller(FillingStrategy<CartrigeBox> fillingStrategy, EventBus eventBus)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));

        _fillingState = new FillingState<CartrigeBox>(_fillingStrategy);

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _eventBus.Subscribe<EnabledSignal<GameWorld>>(Enable);
        _eventBus.Subscribe<DisabledSignal<GameWorld>>(Disable);
        _eventBus.Subscribe<ClearedSignal<GameWorld>>(Clear);

        _isFillingCardEmpty = false;
    }

    private void Clear(ClearedSignal<GameWorld> _)
    {
        _eventBus.Unsubscribe<EnabledSignal<GameWorld>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<GameWorld>>(Disable);
        _eventBus.Unsubscribe<ClearedSignal<GameWorld>>(Clear);

        _fillingStrategy.Clear();
    }

    private void Enable(EnabledSignal<GameWorld> _)
    {
        if (_isFillingCardEmpty == false)
        {
            _fillingState.Enter(OnFillingFinished);
        }
    }

    private void Disable(DisabledSignal<GameWorld> _)
    {
        _fillingState.Exit();
    }

    private void OnFillingFinished()
    {
        _fillingState.Exit();

        _isFillingCardEmpty = true;
    }
}