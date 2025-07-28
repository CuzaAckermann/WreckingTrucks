using System;

public class CartrigeBoxSpace : Space<CartrigeBoxField>, IAmountChangedNotifier
{
    private int _currentLayer;
    private int _currentColumn;

    public CartrigeBoxSpace(CartrigeBoxField field,
                            Mover mover,
                            Filler filler)
                     : base(field,
                            mover,
                            filler)
    {
        _currentLayer = field.AmountLayers - 1;
        _currentColumn = field.AmountColumns - 1;
    }

    public event Action<int> AmountChanged;

    public override void Prepare()
    {
        base.Prepare();
        Field.StopShiftModels();
    }

    public override void Enable()
    {
        Field.AmountCartrigeBoxChanged += OnAmountChanged;
        base.Enable();
    }

    public override void Disable()
    {
        Field.AmountCartrigeBoxChanged -= OnAmountChanged;
        base.Disable();
    }

    public bool TryGetCartrigeBox(out CartrigeBox cartrigeBox)
    {
        if (TryGetFirstCartrigeBox(out cartrigeBox))
        {
            Field.TryRemoveModel(cartrigeBox);
            Field.DecreaseAmountCartrigeBox();
        }

        if (_currentLayer == 0 && _currentColumn == 0)
        {
            Field.ContinueShiftModels();
            Field.StopShiftModels();
        }

        return cartrigeBox != null;
    }

    private bool TryGetFirstCartrigeBox(out CartrigeBox cartrigeBox)
    {
        cartrigeBox = null;

        for (int layer = Field.AmountLayers - 1; layer >= 0; layer--)
        {
            for (int column = Field.AmountColumns - 1; column >= 0; column--)
            {
                if (Field.TryGetFirstModel(layer, column, out Model model))
                {
                    if (model is CartrigeBox)
                    {
                        cartrigeBox = model as CartrigeBox;
                        _currentLayer = layer;
                        _currentColumn = column;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void OnAmountChanged(int amount)
    {
        AmountChanged?.Invoke(amount);
    }
}