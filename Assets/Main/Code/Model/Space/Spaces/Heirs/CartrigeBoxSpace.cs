public class CartrigeBoxSpace : Space<CartrigeBoxField>
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

    public CartrigeBox GetCartrigeBox()
    {
        if (_currentColumn < 0)
        {
            _currentColumn = Field.AmountColumns - 1;
            _currentLayer--;
        }

        if (_currentLayer < 0)
        {
            _currentLayer = Field.AmountLayers - 1;
        }

        if (Field.TryGetFirstModel(_currentLayer, _currentColumn, out Model model))
        {
            if (model is CartrigeBox cartrigeBox)
            {
                Field.TryRemoveModel(cartrigeBox);
                _currentColumn--;
                return cartrigeBox;
            }
        }

        return null;
    }
}