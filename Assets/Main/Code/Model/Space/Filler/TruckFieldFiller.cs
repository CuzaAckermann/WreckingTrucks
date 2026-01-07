using System;

public class TruckFieldFiller
{
    private readonly FillingStrategy<Truck> _fillingStrategy;
    private readonly ModelColorGenerator _modelColorGenerator;
    private readonly Field _field;

    private readonly FillingState<Truck> _fillingState;
    private readonly ModelRemovedWaitingState _modelRemovedWaitingState;

    private bool _isFillingCardEmpty;

    public TruckFieldFiller(Field field, FillingStrategy<Truck> fillingStrategy, ModelColorGenerator modelColorGenerator)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));
        _modelColorGenerator = modelColorGenerator ?? throw new ArgumentNullException(nameof(modelColorGenerator));
        _field = field ?? throw new ArgumentNullException(nameof(field));

        _fillingState = new FillingState<Truck>(_fillingStrategy);
        _modelRemovedWaitingState = new ModelRemovedWaitingState(_field);

        _isFillingCardEmpty = false;
    }

    public void Clear()
    {
        _fillingStrategy.Clear();
    }

    public void Enable()
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

    public void Disable()
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