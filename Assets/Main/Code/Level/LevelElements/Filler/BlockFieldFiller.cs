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

        _eventBus.Subscribe<EnabledSignal<Level>>(Enable);
        _eventBus.Subscribe<DisabledSignal<Level>>(Disable);
        _eventBus.Subscribe<ClearedSignal<Level>>(Clear);

        _isFillingCardEmpty = false;
    }

    public List<ColorType> GetUniqueStoredColors()
    {
        return _fillingStrategy.GetUniqueStoredColors();
    }

    private void Clear(ClearedSignal<Level> _)
    {
        _eventBus.Unsubscribe<EnabledSignal<Level>>(Enable);
        _eventBus.Unsubscribe<DisabledSignal<Level>>(Disable);
        _eventBus.Unsubscribe<ClearedSignal<Level>>(Clear);

        _fillingStrategy.Clear();
    }

    private void Enable(EnabledSignal<Level> _)
    {
        if (_isFillingCardEmpty == false)
        {
            _fillingState.Enter(OnFillingFinished);
        }
    }

    private void Disable(DisabledSignal<Level> _)
    {
        _fillingState.Exit();
    }

    private void OnFillingFinished()
    {
        _fillingState.Exit();

        _isFillingCardEmpty = true;
    }
}