using System;
using System.Collections.Generic;
using UnityEngine;

public class CartrigeBoxField : Field
{
    private const int MinAmountRows = 1;

    private readonly Head _head;
    private readonly Tail _tail;

    private int _amountCartrigeBoxes;
    private bool _isFirstRowTaken;

    public CartrigeBoxField(List<Layer> layers,
                            Vector3 position,
                            Vector3 layerDirection,
                            Vector3 columnDirection,
                            Vector3 rowDirection,
                            float intervalBetweenLayers,
                            float intervalBetweenRows,
                            float intervalBetweenColumns,
                            int amountColumns,
                            int sizeColumn)
                     : base(layers,
                            position,
                            layerDirection,
                            columnDirection,
                            rowDirection,
                            intervalBetweenLayers,
                            intervalBetweenRows,
                            intervalBetweenColumns,
                            amountColumns,
                            sizeColumn)
    {
        //StopShiftModels();

        _head = new Head(new Pointer(AmountLayers - 1, 0, AmountLayers - 1, false),
                         new Pointer(0, 0, AmountColumns - 1, true));

        _tail = new Tail(new Pointer(0, 0, AmountLayers - 1, true),
                         new Pointer(0, 0, AmountColumns - 1, true),
                         new Pointer(0, 0, int.MaxValue, true));

        _amountCartrigeBoxes = 0;
        _isFirstRowTaken = false;
    }

    public override void AddModel(Model model, int indexOfLayer, int indexOfColumn)
    {
        base.AddModel(model, indexOfLayer, indexOfColumn);

        _amountCartrigeBoxes++;
    }

    public bool TryGetFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (TryFindFirstCartrigeBox(out cartrigeBox))
        {
            TryRemoveModel(cartrigeBox);
        }

        if (IsFirstRowEmpty())
        {
            ContinueShiftModels();
            StopShiftModels();

            if (AmountRows > MinAmountRows)
            {
                DecreaseAmountRows();
            }

            _head.Reset();
            _tail.DecreaseCurrentRow();
        }

        if (_isFirstRowTaken == false)
        {
            if (AmountRows == 1)
            {
                _tail.IncreaseCurrentRow();
                IncreaseAmountRows();
            }

            _isFirstRowTaken = true;
        }

        if (cartrigeBox != null)
        {
            _amountCartrigeBoxes--;
        }

        if (_amountCartrigeBoxes == 0)
        {
            _isFirstRowTaken = false;
        }

        Logger.Log(cartrigeBox != null);

        return cartrigeBox != null;
    }

    public FieldPosition GetLastEmptyFieldPosition()
    {
        int indexOfLayer = _tail.CurrentLayer;
        int indexOfColumn = _tail.CurrentColumn;
        int indexOfRow = _tail.CurrentRow;

        if (_tail.TryShift() == false)
        {
            IncreaseAmountRows();
        }

        return new FieldPosition(indexOfLayer, indexOfColumn, indexOfRow);
    }

    private bool IsFirstRowEmpty()
    {
        for (int layer = AmountLayers - 1; layer >= 0; layer--)
        {
            for (int column = 0; column < AmountColumns; column++)
            {
                if (IsEmpty(layer, column, 0) == false)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool TryFindFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        cartrigeBox = null;

        if (TryGetFirstModel(_head.CurrentLayer, _head.CurrentColumn, out Model model))
        {
            if (model is CartrigeBox)
            {
                cartrigeBox = model as CartrigeBox;

                Logger.Log("First try");

                return true;
            }
        }
        else
        {
            Logger.Log("Start Find");
            Logger.Log("CartrigeBox is not found");

            while (_head.TryShift())
            {
                if (TryGetFirstModel(_head.CurrentLayer, _head.CurrentColumn, out model))
                {
                    if (model is CartrigeBox)
                    {
                        cartrigeBox = model as CartrigeBox;

                        return true;
                    }
                }
                else
                {
                    Logger.Log("CartrigeBox is not found");
                }
            }
        }

        return false;
    }
}