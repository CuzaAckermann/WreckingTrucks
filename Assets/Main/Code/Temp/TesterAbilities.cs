using System;
using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    //1 ABILITY ADD CARTRIGE BOX
    private CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;
    private GameButton _actionButton;

    private CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    //2 ABILITY ADD CARTRIGE BOX

    private StopwatchCreator _stopwatchCreator;
    private AmountDisplay _amountDisplay;

    public void Init(CartrigeBoxFillerCreator cartrigeBoxFillerCreator, GameButton actionButton,
                     StopwatchCreator stopwatchCreator, AmountDisplay amountDisplay)
    {
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));
        _actionButton = actionButton ? actionButton : throw new ArgumentNullException(nameof(actionButton));

        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _amountDisplay = amountDisplay ? amountDisplay : throw new ArgumentNullException(nameof(amountDisplay));
    }

    public void Prepare()
    {
        SubscribeToCartrigeBoxFillerCreator();
        SubscribeToActionButton();

        Stopwatch stopwatch = _stopwatchCreator.Create();
        _amountDisplay.Init(stopwatch);
        _amountDisplay.On();
        stopwatch.Start();
    }

    private void SubscribeToCartrigeBoxFillerCreator()
    {
        _cartrigeBoxFillerCreator.Created += OnCreated;
    }

    private void UnsubscribeFromCartrigeBoxFillerCreator()
    {
        _cartrigeBoxFillerCreator.Created -= OnCreated;
    }

    private void SubscribeToActionButton()
    {
        _actionButton.Pressed += OnPressed;
    }

    private void UnsubscribeFromActionButton()
    {
        _actionButton.Pressed -= OnPressed;
    }

    private void OnPressed()
    {
        if (_cartrigeBoxFieldFiller == null)
        {
            return;
        }

        _cartrigeBoxFieldFiller.AddAmountAddedCartrigeBoxes(10);
    }

    private void OnCreated(CartrigeBoxFieldFiller cartrigeBoxFieldFiller)
    {
        _cartrigeBoxFieldFiller = cartrigeBoxFieldFiller;
    }

    

    private void SubscribeTo()
    {

    }

    private void UnsubscribeFrom()
    {

    }
}