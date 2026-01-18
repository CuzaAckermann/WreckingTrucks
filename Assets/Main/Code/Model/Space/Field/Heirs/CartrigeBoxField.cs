using System;
using System.Collections.Generic;
using UnityEngine;

public class CartrigeBoxField : Field
{
    private readonly Head _head;
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
                            int sizeColumn,
                            EventBus eventBus)
                     : base(layers,
                            position,
                            layerDirection,
                            columnDirection,
                            rowDirection,
                            intervalBetweenLayers,
                            intervalBetweenRows,
                            intervalBetweenColumns,
                            amountColumns,
                            sizeColumn,
                            eventBus)
    {
        StopShiftModels();

        _head = new Head(new IndexPointer(AmountLayers - 1, 0, AmountLayers - 1, false),
                         new IndexPointer(0, 0, AmountColumns - 1, true));

        _tail = new Tail(new IndexPointer(0, 0, AmountLayers - 1, true),
                         new IndexPointer(0, 0, AmountColumns - 1, true),
                         new IndexPointer(0, 0, int.MaxValue, true));
    }

    public bool TryGetFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (TryFindFirstCartrigeBox(out cartrigeBox))
        {
            TryRemoveModel(cartrigeBox);

            UpdateField();
        }

        return cartrigeBox != null;
    }

    public IndexPositionInField GetLastEmptyFieldPosition()
    {
        IndexPositionInField indexPositionInField = new IndexPositionInField(_tail.CurrentLayer,
                                                                             _tail.CurrentColumn,
                                                                             _tail.CurrentRow);

        _tail.Shift();

        return indexPositionInField;
    }

    protected override DevastatedFieldSignal InvokeDevastated()
    {
        return new DevastatedCartrigeBoxFieldSignal();
    }

    private bool TryFindFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        cartrigeBox = null;

        do
        {
            if (TryGetFirstModel(_head.CurrentLayer, _head.CurrentColumn, out Model model))
            {
                if (model is CartrigeBox)
                {
                    cartrigeBox = model as CartrigeBox;

                    return true;
                }
            }
        }
        while (_head.TryShift());

        return false;
    }

    private void UpdateField()
    {
        if (CurrentAmount == 0)
        {
            _tail.ResetAll();

            // только для того чтобы вызвалось событие, что поле пустое
            UpdateFirstRow();

            return;
        }

        if (IsFirstRowEmpty())
        {
            _tail.DecreaseCurrentRow();

            UpdateFirstRow();

            return;
        }

        if (_tail.CurrentRow == 0)
        {
            _tail.IncreaseCurrentRow();
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

    private void UpdateFirstRow()
    {
        ContinueShiftModels();
        StopShiftModels();

        _head.Reset();
    }
}