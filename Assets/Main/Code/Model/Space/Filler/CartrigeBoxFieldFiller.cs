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

        _eventBus.Subscribe<EnabledGameWorldSignal>(Enable);
        _eventBus.Subscribe<DisabledGameWorldSignal>(Disable);
        _eventBus.Subscribe<DestroyedGameWorldSignal>(Clear);

        _isFillingCardEmpty = false;
    }

    private void Clear(DestroyedGameWorldSignal _)
    {
        _eventBus.Unsubscribe<EnabledGameWorldSignal>(Enable);
        _eventBus.Unsubscribe<DisabledGameWorldSignal>(Disable);
        _eventBus.Unsubscribe<DestroyedGameWorldSignal>(Clear);

        _fillingStrategy.Clear();
    }

    private void Enable(EnabledGameWorldSignal _)
    {
        if (_isFillingCardEmpty == false)
        {
            _fillingState.Enter(OnFillingFinished);
        }
    }

    private void Disable(DisabledGameWorldSignal _)
    {
        _fillingState.Exit();
    }

    private void OnFillingFinished()
    {
        _fillingState.Exit();

        _isFillingCardEmpty = true;
    }
}