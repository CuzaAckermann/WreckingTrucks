public class CartrigeBoxFactory : ModelFactory<CartrigeBox>
{
    public CartrigeBoxFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                       : base(factorySettings, modelSettings)
    {
        InitializePool(factorySettings.InitialPoolSize,
                       factorySettings.MaxPoolCapacity);
    }

    protected override CartrigeBox CreateElement()
    {
        return new CartrigeBox(ModelSettings.Movespeed, ModelSettings.Rotatespeed);
    }
}