using System;
using System.Collections.Generic;

public class BlockFieldFiller
{
    private readonly FillingStrategy<Block> _fillingStrategy;

    private readonly FillingState<Block> _fillingState;

    private readonly EventBus _eventBus;

    private bool _isFillingCardEmpty;

    public BlockFieldFiller(FillingStrategy<Block> fillingStrategy,
                            EventBus eventBus)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));

        _fillingState = new FillingState<Block>(_fillingStrategy);

        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        _eventBus.Subscribe<EnabledGameWorldSignal>(Enable);
        _eventBus.Subscribe<DisabledGameWorldSignal>(Disable);
        _eventBus.Subscribe<DestroyedGameWorldSignal>(Clear);

        _isFillingCardEmpty = false;
    }

    public List<ColorType> GetUniqueStoredColors()
    {
        return _fillingStrategy.GetUniqueStoredColors();
    }

    public void Clear(DestroyedGameWorldSignal _)
    {
        _eventBus.Unsubscribe<EnabledGameWorldSignal>(Enable);
        _eventBus.Unsubscribe<DisabledGameWorldSignal>(Disable);
        _eventBus.Unsubscribe<DestroyedGameWorldSignal>(Clear);

        _fillingStrategy.Clear();
    }

    public void Enable(EnabledGameWorldSignal _)
    {
        if (_isFillingCardEmpty == false)
        {
            _fillingState.Enter(OnFillingFinished);
        }
    }

    public void Disable(DisabledGameWorldSignal _)
    {
        _fillingState.Exit();
    }

    private void OnFillingFinished()
    {
        _fillingState.Exit();

        _isFillingCardEmpty = true;
    }
}