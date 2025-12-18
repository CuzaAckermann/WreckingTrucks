using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
        ResetHead();

        CurrentLayerTail = 0;
        CurrentColumnTail = 0;
        CurrentRowTail = 0;

        _needShift = false;
    }

    public int CurrentLayerTail { get; private set; }

    public int CurrentColumnTail { get; private set; }

    public int CurrentRowTail { get; private set; }

    public void AddCartrigeBox(Model cartrigeBox)
    {
        InsertModel(cartrigeBox, CurrentLayerTail, CurrentColumnTail, CurrentRowTail);

        bool isFirstRowsEmpty = true;

        for (int layer = AmountLayers - 1; layer >= 0; layer--)
        {
            for (int column = 0; column < AmountColumns; column++)
            {
                if (TryGetFirstModel(layer, column, out Model _))
                {
                    isFirstRowsEmpty = false;
                    break;
                }
            }
        }

        if (isFirstRowsEmpty)
        {
            ContinueShiftModels();

            Logger.Log(CurrentRowTail);

            if (CurrentRowTail > 1)
            {
                CurrentRowTail--;
            }
            else
            {
                ResetTail();
            }

            StopShiftModels();
        }
        else
        {
            ShiftTail();
        }
    }

    //public override void AddModel(Model model, int indexOfLayer, int indexOfColumn)
    //{
    //    base.AddModel(model, indexOfLayer, indexOfColumn);

    //    ShiftTail();
    //}

    public bool TryGetCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (TryGetFirstCartrigeBox(out cartrigeBox))
        {
            TryRemoveModel(cartrigeBox);
        }

        DefineNextHead();

        if (_needShift)
        {
            ContinueShiftModels();

            if (CurrentRowTail > 1)
            {
                CurrentRowTail--;
            }
            else
            {
                ResetTail();
            }

            StopShiftModels();
            _needShift = false;
        }

        return cartrigeBox != null;
    }

    private bool TryGetFirstCartrigeBox(out CartrigeBox cartrigeBox)
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

                        _currentColumnHead = column;
                        _currentLayerHead = layer;

                        return true;
                    }
                }
                else
                {
                    //Logger.Log("Object was not received");
                }
            }
        }

        //if (TryGetFirstModel(_currentLayerHead, _currentColumnHead, out Model model))
        //{
        //    if (model is CartrigeBox)
        //    {
        //        cartrigeBox = model as CartrigeBox;
        //    }
        //}
        //else
        //{
        //    Logger.Log("Object was not received");
        //}

        return false;
    }

    private void DefineNextHead()
    {
        //_needShift = true;

        //for (int layer = AmountLayers - 1; layer >= 0; layer--)
        //{
        //    for (int column = 0; column < AmountColumns; column++)
        //    {
        //        if (TryGetFirstModel(layer, column, out Model _))
        //        {
        //            _currentColumnHead = column;
        //            _currentLayerHead = layer;
        //            _needShift = false;
        //            return;
        //        }
        //    }
        //}

        while (TryFindHead() == false && _needShift == false)
        {
            //Logger.Log("Not found");
        }
    }

    private bool TryFindHead()
    {
        _currentColumnHead++;

        if (_currentColumnHead >= AmountColumns)
        {
            _currentColumnHead = 0;

            _currentLayerHead--;

            if (_currentLayerHead < 0)
            {
                ResetHead();
                _needShift = true;

                return false;
            }
        }

        return TryGetFirstModel(_currentLayerHead, _currentColumnHead, out Model _);
    }

    private void ShiftTail()
    {
        CurrentColumnTail++;

        if (CurrentColumnTail < AmountColumns)
        {
            return;
        }

        CurrentColumnTail = 0;

        CurrentLayerTail++;

        if (CurrentLayerTail < AmountLayers)
        {
            return;
        }

        CurrentLayerTail = 0;

        CurrentRowTail++;
    }

    private void ResetTail()
    {
        CurrentColumnTail = 0;
        CurrentLayerTail = 0;
    }

    private void ResetHead()
    {
        _currentLayerHead = AmountLayers - 1;
        _currentColumnHead = 0;
    }
}