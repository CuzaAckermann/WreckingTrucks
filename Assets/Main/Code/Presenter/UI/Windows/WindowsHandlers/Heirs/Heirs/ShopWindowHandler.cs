public class ShopWindowHandler : WindowHandler<ShopWindow>
{
    public ShopWindowHandler(ShopWindow stateWindow, InputStateStorage storage) : base(stateWindow, storage)
    {

    }

    protected override void SubscribeToWindow(ShopWindow window)
    {
        window.ReturnButton.Pressed += OnInputStateReturning;
    }

    protected override void UnsubscribeFromWindow(ShopWindow window)
    {
        window.ReturnButton.Pressed -= OnInputStateReturning;
    }
}