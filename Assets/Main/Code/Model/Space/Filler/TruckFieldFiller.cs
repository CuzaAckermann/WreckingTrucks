using System;

public class TruckFieldFiller
{
    private readonly FillingStrategy _fillingStrategy;
    private readonly TruckGenerator _truckGenerator;
    private readonly Field _field;

    private bool _isFillingCardEmpty;

    private bool _isSubscribedToFillingStrategy;
    private bool _isSubscribedToField;

    public TruckFieldFiller(Field field, FillingStrategy fillingStrategy, TruckGenerator truckGenerator)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));
        _truckGenerator = truckGenerator ?? throw new ArgumentNullException(nameof(truckGenerator));
        _field = field ?? throw new ArgumentNullException(nameof(field));

        _isFillingCardEmpty = false;

        _isSubscribedToFillingStrategy = false;
        _isSubscribedToField = false;
    }

    public void Clear()
    {
        _fillingStrategy.Clear();
    }

    public void Enable()
    {
        if (_isFillingCardEmpty == false && _isSubscribedToFillingStrategy == false)
        {
            SubscribeToFillingStrategy();
            _fillingStrategy.Enable();

        }
        else if (_isFillingCardEmpty)
        {
            SubscribeToField();
        }
    }

    public void Disable()
    {
        if (_isSubscribedToFillingStrategy)
        {
            _fillingStrategy.Disable();
            UnsubscribeFromFillingStrategy();
        }
        else if (_isSubscribedToField)
        {
            UnsubscribeFromField();
        }
    }

    private void SubscribeToFillingStrategy()
    {
        _fillingStrategy.FillingCardEmpty += OnFillingFinished;
        _isSubscribedToFillingStrategy = true;
    }

    private void UnsubscribeFromFillingStrategy()
    {
        _fillingStrategy.FillingCardEmpty -= OnFillingFinished;
        _isSubscribedToFillingStrategy = false;
    }

    private void OnFillingFinished()
    {
        Disable();

        SubscribeToField();
        _isFillingCardEmpty = true;
    }

    private void SubscribeToField()
    {
        _field.ModelRemoved += OnModelRemoved;
        _isSubscribedToField = true;
    }

    private void UnsubscribeFromField()
    {
        _field.ModelRemoved -= OnModelRemoved;
        _isSubscribedToField = false;
    }

    private void OnModelRemoved(int layer, int column, int _)
    {
        _fillingStrategy.PlaceModel(_truckGenerator.Generate(), layer, column);
    }
}