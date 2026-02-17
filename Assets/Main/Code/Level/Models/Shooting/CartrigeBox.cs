public class CartrigeBox : Model
{
    public CartrigeBox(Placeable positionManipulator,
                       IMover mover,
                       IRotator rotator)
                : base(positionManipulator,
                       mover,
                       rotator)
    {

    }
}