using System;
using UnityEngine;

public class CartrigeBoxFieldFiller
{
    //private readonly FillingStrategy _fillingStrategy;
    //private readonly CartrigeBoxField _field;
    //private readonly CartrigeBoxFactory _cartrigeBoxFactory;

    //private bool _isFillingCardEmpty;

    //private bool _isSubscribedToFillingStrategy;

    //public CartrigeBoxFieldFiller(CartrigeBoxField field,
    //                              FillingStrategy fillingStrategy/*,
    //                              CartrigeBoxFactory cartrigeBoxFactory*/)
    //{
    //    _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));
    //    _field = field ?? throw new ArgumentNullException(nameof(field));
    //    //_cartrigeBoxFactory = cartrigeBoxFactory ?? throw new ArgumentException(nameof(cartrigeBoxFactory));

    //    _isFillingCardEmpty = false;

    //    _isSubscribedToFillingStrategy = false;
    //}

    //public void Clear()
    //{
    //    _fillingStrategy.Clear();
    //}

    //public void Enable()
    //{
    //    if (_isFillingCardEmpty == false && _isSubscribedToFillingStrategy == false)
    //    {
    //        SubscribeToFillingStrategy();
    //        _fillingStrategy.Enable();
    //    }
    //}

    //public void Disable()
    //{
    //    if (_isSubscribedToFillingStrategy)
    //    {
    //        _fillingStrategy.Disable();
    //        UnsubscribeFromFillingStrategy();
    //    }
    //}

    //private void SubscribeToFillingStrategy()
    //{
    //    _fillingStrategy.FillingCardEmpty += OnFillingFinished;
    //    _isSubscribedToFillingStrategy = true;
    //}

    //private void UnsubscribeFromFillingStrategy()
    //{
    //    _fillingStrategy.FillingCardEmpty -= OnFillingFinished;
    //    _isSubscribedToFillingStrategy = false;
    //}

    //private void OnFillingFinished()
    //{
    //    Disable();

    //    _isFillingCardEmpty = true;
    //}

    private readonly Stopwatch _stopwatch;
    private readonly CartrigeBoxField _field;
    private readonly CartrigeBoxFactory _cartrigeBoxFactory;

    private int _amountAddedCartrigeBoxes;

    public CartrigeBoxFieldFiller(Stopwatch stopwatch,
                                  float frequency,
                                  CartrigeBoxField field,
                                  CartrigeBoxFactory cartrigeBoxFactory,
                                  int amountAddedCartrigeBoxes)
    {
        if (amountAddedCartrigeBoxes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountAddedCartrigeBoxes));
        }

        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _stopwatch.SetNotificationInterval(frequency);

        _field = field ?? throw new ArgumentNullException(nameof(field));
        _cartrigeBoxFactory = cartrigeBoxFactory ?? throw new ArgumentException(nameof(cartrigeBoxFactory));
        _amountAddedCartrigeBoxes = amountAddedCartrigeBoxes;
    }

    public void Clear()
    {

    }

    public void Enable()
    {
        if (_amountAddedCartrigeBoxes <= 0)
        {
            return;
        }

        _stopwatch.IntervalPassed += AddCartrigeBox;
        _stopwatch.Start();
    }

    public void Disable()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= AddCartrigeBox;
    }

    private void AddCartrigeBox()
    {
        CartrigeBox cartrigeBox = _cartrigeBoxFactory.Create();
        cartrigeBox.SetColor(ColorType.Gray);

        PlaceModel(new RecordPlaceableModel(cartrigeBox,
                                            _field.CurrentLayerTail,
                                            _field.CurrentColumnTail,
                                            _field.CurrentRowTail));

        _field.AddCartrigeBox();

        _amountAddedCartrigeBoxes--;

        if (_amountAddedCartrigeBoxes <= 0)
        {
            Disable();
        }
    }

    private void PlaceModel(RecordPlaceableModel record)
    {
        Vector3 spawnPosition = GetSpawnPosition(record);

        spawnPosition += _field.Position;

        record.PlaceableModel.SetFirstPosition(spawnPosition);

        _field.AddModel(record.PlaceableModel,
                        record.IndexOfLayer,
                        record.IndexOfColumn);
    }

    private Vector3 GetSpawnPosition(RecordPlaceableModel record)
    {
        return _field.Right * record.IndexOfColumn * _field.IntervalBetweenColumns +
               _field.Up * record.IndexOfLayer * _field.IntervalBetweenLayers +
               _field.Forward * (_field.AmountRows + 1) * _field.IntervalBetweenRows;
    }
}