public class CartrigeBoxGenerator : ModelGenerator<CartrigeBox>
{
    public CartrigeBoxGenerator(CartrigeBoxFactory cartrigeBoxFactory,
                                ColorGenerator colorGenerator)
                         : base(cartrigeBoxFactory,
                                colorGenerator)
    {

    }
}