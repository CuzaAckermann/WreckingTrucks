using System;
using System.Collections.Generic;
using UnityEngine;

public class Dispencer : IRecordStorage
{
    private readonly CartrigeBoxField _field;

    private int _amountAddedCartrigeBoxes;

    public Dispencer(CartrigeBoxField field, int amountAddedCartrigeBoxes)
    {
        if (amountAddedCartrigeBoxes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountAddedCartrigeBoxes));
        }

        _field = field ?? throw new ArgumentNullException(nameof(field));

        _amountAddedCartrigeBoxes = amountAddedCartrigeBoxes;
    }

    public event Action RecordAppeared;

    public int Amount => _amountAddedCartrigeBoxes;

    public void Clear()
    {
        Logger.Log("Method is empty");
    }

    public IReadOnlyList<ColorType> GetUniqueStoredColors()
    {
        return new List<ColorType> { ColorType.Gray };
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

        RecordAppeared?.Invoke();
    }

    public bool TryGetNextRecord(out RecordPlaceableModel record)
    {
        record = null;

        if (_amountAddedCartrigeBoxes <= 0)
        {
            return false;
        }

        FieldPosition lastEmpty = _field.GetLastEmptyFieldPosition();

        record = new RecordPlaceableModel(ColorType.Gray,
                                          lastEmpty.IndexOfLayer,
                                          lastEmpty.IndexOfColumn,
                                          lastEmpty.IndexOfRow);

        _amountAddedCartrigeBoxes--;

        return true;
    }

    public bool TryGetCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (_field.TryGetFirstCartrigeBox(out cartrigeBox) == false)
        {
            Logger.Log("CartrigeBoxField is empty");
        }

        return cartrigeBox != null;
    }
}