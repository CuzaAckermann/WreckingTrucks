public class CartrigeBoxFactory : ModelFactory<CartrigeBox>
{
    public CartrigeBoxFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                       : base(factorySettings, modelSettings)
    {
        InitPool(factorySettings.InitialPoolSize,
                 factorySettings.MaxPoolCapacity);
    }

    protected override CartrigeBox CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new CartrigeBox(positionManipulator,
                               MoverCreator.Create(positionManipulator),
                               RotatorCreator.Create(positionManipulator));
    }
}