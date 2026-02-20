public class CartrigeBoxFactory : ModelFactory<CartrigeBox>
{
    public CartrigeBoxFactory(FactorySettings factorySettings, ModelSettings modelSettings)
                       : base(factorySettings, modelSettings)
    {

    }

    protected override IDestroyable CreateElement()
    {
        Placeable positionManipulator = new Placeable();

        return new CartrigeBox(positionManipulator,
                               MoverCreator.Create(positionManipulator),
                               RotatorCreator.Create(positionManipulator));
    }
}