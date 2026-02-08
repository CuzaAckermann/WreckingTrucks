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
        PositionManipulator positionManipulator = new PositionManipulator();

        return new CartrigeBox(positionManipulator,
                               MoverCreator.Create(positionManipulator),
                               RotatorCreator.Create(positionManipulator));
    }
}