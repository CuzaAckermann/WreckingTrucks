using System.Collections.Generic;
using UnityEngine;

public class CartrigeBoxField : Field
{
    private int _currentLayerHead;
    private int _currentColumnHead;

    private bool _needShift;

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

        _needShift = false;
    }

    public int CurrentLayerTail { get; private set; }

    public int CurrentColumnTail { get; private set; }

    public override void AddModel(Model model, int indexOfLayer, int indexOfColumn)
    {
        base.AddModel(model, indexOfLayer, indexOfColumn);

        ShiftTail();
    }

    public bool TryGetCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (TryGetFirstCartrigeBox(out cartrigeBox))
        {
            TryRemoveModel(cartrigeBox);
        }

        ShiftHead();

        if (_needShift)
        {
            ContinueShiftModels();
            StopShiftModels();
            _needShift = false;
        }

        return cartrigeBox != null;
    }

    private bool TryGetFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        cartrigeBox = null;

        if (TryGetFirstModel(_currentLayerHead, _currentColumnHead, out Model model))
        {
            if (model is CartrigeBox)
            {
                cartrigeBox = model as CartrigeBox;
            }
        }
        else
        {
            Logger.Log("Object was not received");
        }

        return cartrigeBox != null;
    }

    private void ShiftHead()
    {
        _currentColumnHead--;

        if (_currentColumnHead < 0)
        {
            _currentColumnHead = AmountColumns - 1;

            _currentLayerHead--;

            if (_currentLayerHead < 0)
            {
                _currentLayerHead = AmountLayers - 1;
                _needShift = true;
            }
        }
    }

    private void ShiftTail()
    {
        CurrentColumnTail++;

        if (CurrentColumnTail >= AmountColumns)
        {
            CurrentColumnTail = 0;

            CurrentLayerTail++;

            if (CurrentLayerTail >= AmountLayers)
            {
                CurrentLayerTail = 0;
            }
        }
    }
}