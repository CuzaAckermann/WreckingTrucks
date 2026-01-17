using System;

public class CreatedCartrigeBoxFieldSignal
{
    private readonly CartrigeBoxField _cartrigeBoxField;

    public CreatedCartrigeBoxFieldSignal(CartrigeBoxField cartrigeBoxField)
    {
        _cartrigeBoxField = cartrigeBoxField ?? throw new ArgumentNullException(nameof(cartrigeBoxField));
    }

    public CartrigeBoxField CartrigeBoxField => _cartrigeBoxField;
}