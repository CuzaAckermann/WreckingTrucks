public class CartrigeBox : Model
{
    public CartrigeBox(PositionManipulator positionManipulator,
                       IMover mover,
                       IRotator rotator)
                : base(positionManipulator,
                       mover,
                       rotator)
    {

    }
}