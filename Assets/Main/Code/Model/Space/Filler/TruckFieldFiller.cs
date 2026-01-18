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
        else
        {
            _modelRemovedWaitingState.Enter(OnModelRemoved);
        }
    }

    private void Disable(DisabledGameWorldSignal _)
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