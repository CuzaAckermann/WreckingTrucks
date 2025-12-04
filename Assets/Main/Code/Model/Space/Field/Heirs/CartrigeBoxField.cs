using System;
using System.Collections.Generic;
using UnityEngine;

public class CartrigeBoxField : Field
{
    private int _currentLayerHead;
    private int _currentColumnHead;

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
        _currentLayerHead = AmountLayers - 1;
        _currentColumnHead = AmountColumns - 1;

        CurrentLayerTail = 0;
        CurrentColumnTail = 0;
        CurrentRowTail = 0;
    }

    public int CurrentLayerTail { get; private set; }

    public int CurrentColumnTail { get; private set; }

    public int CurrentRowTail { get; private set; }

    public void ShiftTail()
    {
        CurrentColumnTail++;

        if (CurrentColumnTail >= AmountColumns)
        {
            CurrentColumnTail = 0;

            CurrentLayerTail++;

            if (CurrentLayerTail >= AmountLayers)
            {
                CurrentLayerTail = 0;

                CurrentRowTail++;

                if (CurrentRowTail >= AmountRows)
                {
                    IncreaseAmountRows();
                }
            }
        }
    }

    public bool TryGetCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (TryGetFirstCartrigeBox(out cartrigeBox))
        {
            TryRemoveModel(cartrigeBox);
        }

        if (_currentLayerHead == 0 && _currentColumnHead == 0)
        {
            ContinueShiftModels();
            StopShiftModels();
        }

        return cartrigeBox != null;
    }

    private bool TryGetFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        cartrigeBox = null;

        for (int layer = AmountLayers - 1; layer >= 0; layer--)
        {
            for (int column = AmountColumns - 1; column >= 0; column--)
            {
                if (TryGetFirstModel(layer, column, out Model model))
                {
                    if (model is CartrigeBox)
                    {
                        cartrigeBox = model as CartrigeBox;
                        _currentLayerHead = layer;
                        _currentColumnHead = column;
                        return true;
                    }
                }
            }
        }

        return false;
    }
}