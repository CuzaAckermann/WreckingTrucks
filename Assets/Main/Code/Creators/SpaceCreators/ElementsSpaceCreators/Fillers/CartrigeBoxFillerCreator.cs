using System;

public class CartrigeBoxFillerCreator
{
    private readonly FillingStrategiesCreator _fillingStrategiesCreator;

    public CartrigeBoxFillerCreator(FillingStrategiesCreator fillingStrategiesCreator)
    {
        _fillingStrategiesCreator = fillingStrategiesCreator ?? throw new ArgumentNullException(nameof(fillingStrategiesCreator));
    }

    public event Action<CartrigeBoxFieldFiller> Created;

    public CartrigeBoxFieldFiller Create(CartrigeBoxField cartrigeBoxField, Dispencer dispencer)
    {
        FillingStrategy<CartrigeBox> fillingStrategy = _fillingStrategiesCreator.Create<CartrigeBox>(cartrigeBoxField, dispencer);
        fillingStrategy.ActivateNonstopFilling();

        CartrigeBoxFieldFiller cartrigeBoxFieldFiller = new CartrigeBoxFieldFiller(fillingStrategy);

        Created?.Invoke(cartrigeBoxFieldFiller);

        return cartrigeBoxFieldFiller;
    }
}