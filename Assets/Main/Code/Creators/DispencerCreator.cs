public class DispencerCreator
{
    public Dispencer Create(CartrigeBoxField cartrigeBoxField, int startAmountCartrigeBoxes, EventBus eventBus)
    {
        Dispencer dispencer = new Dispencer(cartrigeBoxField, startAmountCartrigeBoxes, eventBus);

        eventBus.Invoke(new CreatedDispencerSignal(dispencer));

        return dispencer;
    }
}