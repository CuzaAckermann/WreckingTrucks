using System;

public class TruckFieldFiller
{
    private readonly FillingStrategy _fillingStrategy;
    private readonly TruckGenerator _truckGenerator;
    private readonly Field _field;

    private readonly FillingState _fillingState;
    private readonly ModelRemovedWaitingState _modelRemovedWaitingState;

    private bool _isFillingCardEmpty;

    public TruckFieldFiller(Field field, FillingStrategy fillingStrategy, TruckGenerator truckGenerator)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));
        _truckGenerator = truckGenerator ?? throw new ArgumentNullException(nameof(truckGenerator));
        _field = field ?? throw new ArgumentNullException(nameof(field));

        _fillingState = new FillingState(_fillingStrategy);
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
        else if (_isFillingCardEmpty)
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
        _truckGenerator.AddRecord(indexOflayer, indexOfColumn);
    }
}