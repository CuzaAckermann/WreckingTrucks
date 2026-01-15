using System;
using System.Collections.Generic;
using UnityEngine;

public class CartrigeBoxField : Field
{
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
        DecreaseAmountRows();

        _head = new Head(new IndexPointer(AmountLayers - 1, 0, AmountLayers - 1, false),
                         new IndexPointer(0, 0, AmountColumns - 1, true));

        _tail = new Tail(new IndexPointer(0, 0, AmountLayers - 1, true),
                         new IndexPointer(0, 0, AmountColumns - 1, true),
                         new IndexPointer(0, 0, int.MaxValue, true));

        _amountCartrigeBoxes = 0;
        _isFirstRowTaken = false;
    }

    public event Action CartrigeBoxAppeared;

    public override void InsertModel(Model model, int indexOfLayer, int indexOfColumn, int indexOfRow)
    {
        base.InsertModel(model, indexOfLayer, indexOfColumn, indexOfRow);

        bool isEmpty = _amountCartrigeBoxes == 0;

        _amountCartrigeBoxes++;

        if (isEmpty)
        {
            CartrigeBoxAppeared?.Invoke();
        }
    }

    public bool TryGetFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (TryFindFirstCartrigeBox(out cartrigeBox))
        {
            TryRemoveModel(cartrigeBox);

            _amountCartrigeBoxes--;
        }

        if (_amountCartrigeBoxes > 0)
        {
            if (TryUpdateRows())
            {
                _isFirstRowTaken = false;
            }
            else
            {
                _isFirstRowTaken = true;
            }
        }
        else if (_amountCartrigeBoxes == 0)
        {
            DecreaseAmountRows();

            _head.Reset();

            _tail.DecreaseCurrentRow();
            _tail.Reset();

            // только для того чтобы вызвалось событие, что поле пустое
            StepShift();

            _isFirstRowTaken = false;
        }
        else
        {
            Logger.Log("Impossible situation");
        }

        return cartrigeBox != null;
    }

    public IndexPositionInField GetLastEmptyFieldPosition()
    {
        if (AmountRows == 0 && _isFirstRowTaken)
        {
            _tail.IncreaseCurrentRow();
            _tail.Reset();

            IncreaseAmountRows();
        }

        int indexOfLayer = _tail.CurrentLayer;
        int indexOfColumn = _tail.CurrentColumn;
        int indexOfRow = _tail.CurrentRow;

        if (_tail.TryShift() == false)
        {
            IncreaseAmountRows();
        }

        return new IndexPositionInField(indexOfLayer, indexOfColumn, indexOfRow);
    }

    private bool TryFindFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        cartrigeBox = null;

        if (TryGetFirstModel(_head.CurrentLayer, _head.CurrentColumn, out Model model))
        {
            if (model is CartrigeBox)
            {
                cartrigeBox = model as CartrigeBox;

                //Logger.Log("First try");

                return true;
            }
        }
        else
        {
            //Logger.Log("Start Find");
            //Logger.Log("CartrigeBox is not found");

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
                    //Logger.Log("CartrigeBox is not found");
                }
            }
        }

        return false;
    }

    private bool TryUpdateRows()
    {
        if (IsFirstRowEmpty())
        {
            StepShift();

            DecreaseAmountRows();

            _head.Reset();
            _tail.DecreaseCurrentRow();

            return true;
        }

        return false;
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

    private void StepShift()
    {
        ContinueShiftModels();
        StopShiftModels();
    }
}