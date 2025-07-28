using System;
using System.Collections.Generic;
using UnityEngine;

public class CartrigeBoxField : Field
{
    private int _amountCartrigeBox;

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
        _amountCartrigeBox = 0;
    }

    public event Action<int> AmountCartrigeBoxChanged;

    public void DecreaseAmountCartrigeBox()
    {
        _amountCartrigeBox--;
        AmountCartrigeBoxChanged.Invoke(_amountCartrigeBox);
    }

    protected override void OnModelAdded(Model model)
    {
        base.OnModelAdded(model);
        _amountCartrigeBox++;
        AmountCartrigeBoxChanged?.Invoke(_amountCartrigeBox);
    }
}