using System;
using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    //1 ABILITY ADD CARTRIGE BOX
    private CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;
    private GameButton _actionButton;

    private CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    //2 DISPLAY GLOBAL STOPWATCH

    private StopwatchCreator _stopwatchCreator;
    private TimeDisplay _timeDisplay;

    public void Init(CartrigeBoxFillerCreator cartrigeBoxFillerCreator, GameButton actionButton,
                     StopwatchCreator stopwatchCreator, TimeDisplay timeDisplay)
    {
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));
        _actionButton = actionButton ? actionButton : throw new ArgumentNullException(nameof(actionButton));

        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _timeDisplay = timeDisplay ? timeDisplay : throw new ArgumentNullException(nameof(timeDisplay));
    }

    public void Prepare()
    {
        SubscribeToCartrigeBoxFillerCreator();
        SubscribeToActionButton();

        PrepareGlobalStopwatch();
    }

    public void Disable()
    {
        UnsubscribeFromCartrigeBoxFillerCreator();
        UnsubscribeFromActionButton();
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

    private void PrepareGlobalStopwatch()
    {
        Stopwatch stopwatch = _stopwatchCreator.Create();
        _timeDisplay.Init(stopwatch);
        _timeDisplay.On();
        stopwatch.Start();
    }

    private void SubscribeTo()
    {

    }

    private void UnsubscribeFrom()
    {

    }
}