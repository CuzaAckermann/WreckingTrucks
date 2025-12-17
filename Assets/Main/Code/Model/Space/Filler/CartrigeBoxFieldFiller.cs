using System;
using UnityEngine;

public class CartrigeBoxFieldFiller
{
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

    public void AddAmountAddedCartrigeBoxes(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        bool hasRemains = _amountAddedCartrigeBoxes > 0;

        _amountAddedCartrigeBoxes += amount;

        if (hasRemains)
        {
            return;
        }

        Enable();
    }

    private void AddCartrigeBox()
    {
        CartrigeBox cartrigeBox = _cartrigeBoxFactory.Create();
        cartrigeBox.SetColor(ColorType.Gray);

        PlaceModel(new RecordPlaceableModel(cartrigeBox,
                                            _field.CurrentLayerTail,
                                            _field.CurrentColumnTail,
                                            _field.GetAmountModelsInColumn(_field.CurrentLayerTail, _field.CurrentColumnTail))); // Попробовать _field.GetAmountModelsInColumn()

        _amountAddedCartrigeBoxes--;

        if (_amountAddedCartrigeBoxes <= 0)
        {
            Disable();
            _stopwatch.Reset();
        }
    }

    private void PlaceModel(RecordPlaceableModel record)
    {
        Vector3 spawnPosition = GetSpawnPosition(record);

        spawnPosition += _field.Position;

        record.PlaceableModel.SetFirstPosition(spawnPosition);

        //_field.AddModel(record.PlaceableModel,
        //                record.IndexOfLayer,
        //                record.IndexOfColumn);

        _field.AddCartrigeBox(record.PlaceableModel);
    }

    private Vector3 GetSpawnPosition(RecordPlaceableModel record)
    {
        return _field.Right * record.IndexOfColumn * _field.IntervalBetweenColumns +
               _field.Up * record.IndexOfLayer * _field.IntervalBetweenLayers +
               _field.Forward * (_field.GetAmountModelsInColumn(record.IndexOfLayer, record.IndexOfColumn) + 1) * _field.IntervalBetweenRows;
    }
}