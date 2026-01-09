using System;
using System.Collections.Generic;
using UnityEngine;

public class CartrigeBoxField : Field
{
    private const int MinAmountRows = 1;

    private readonly Tail _tail;

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

        _tail = new Tail(new Pointer(0, 0, AmountLayers - 1, true),
                         new Pointer(0, 0, AmountColumns - 1, true),
                         new Pointer(0, MinAmountRows, int.MaxValue, true));
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

            _tail.DecreaseCurrentRow();
        }

        return cartrigeBox != null;
    }

    public void GetLastEmpty(out int indexOfLayer, out int indexOfColumn, out int indexOfRow)
    {
        indexOfLayer = _tail.CurrentLayer;
        indexOfColumn = _tail.CurrentColumn;
        indexOfRow = _tail.CurrentRow;

        if (_tail.TryShift() == false)
        {
            IncreaseAmountRows();
        }
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

        for (int layer = AmountLayers - 1; layer >= 0; layer--)
        {
            for (int column = 0; column < AmountColumns; column++)
            {
                if (TryGetFirstModel(layer, column, out Model model))
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
}