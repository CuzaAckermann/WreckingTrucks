using System;

public class TruckFieldFiller
{
    private readonly FillingStrategy<Truck> _fillingStrategy;
    private readonly ModelColorGenerator _modelColorGenerator;
    private readonly Field _field;

    private readonly FillingState<Truck> _fillingState;
    private readonly ModelRemovedWaitingState _modelRemovedWaitingState;

    private readonly EventBus _eventBus;

    private bool _isFillingCardEmpty;

    public TruckFieldFiller(Field field,
                            FillingStrategy<Truck> fillingStrategy,
                            ModelColorGenerator modelColorGenerator,
                            EventBus eventBus)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));
        _modelColorGenerator = modelColorGenerator ?? throw new ArgumentNullException(nameof(modelColorGenerator));
        _field = field ?? throw new ArgumentNullException(nameof(field));

        _fillingState = new FillingState<Truck>(_fillingStrategy);
        _modelRemovedWaitingState = new ModelRemovedWaitingState(_field);

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
        else
        {
            _modelRemovedWaitingState.Enter(OnModelRemoved);
        }
    }

    private void Disable(DisabledSignal<GameWorld> _)
    {
        _fillingState.Exit();
        _modelRemovedWaitingState.Exit();
    }

    private void OnFillingFinished()
    {
        _fillingState.Exit();

        _isFillingCardEmpty = true;

        _modelRemovedWaitingState.Enter(OnModelRemoved);
    }

    private void OnModelRemoved(int indexOflayer, int indexOfColumn, int _)
    {
        _modelColorGenerator.AddRecord(indexOflayer, indexOfColumn);
    }
}