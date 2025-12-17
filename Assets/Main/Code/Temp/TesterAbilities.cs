using System;
using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    //1 ABILITY ADD CARTRIGE BOX
    private CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;
    private GameButton _addButton;

    private CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    private int _amountAdditionalCartrigeBox = 20;

    //2 DISPLAY GLOBAL STOPWATCH

    private StopwatchCreator _stopwatchCreator;
    private TimeDisplay _timeDisplay;

    //3 ABILITY REMOVE CARTRIGE BOX

    private CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;
    private GameButton _takeButton;

    private CartrigeBoxField _cartrigeBoxField;

    private int _amountTakenCartrigeBox = 2;

    public void Init(CartrigeBoxFillerCreator cartrigeBoxFillerCreator, GameButton addButton,
                     StopwatchCreator stopwatchCreator, TimeDisplay timeDisplay,
                     CartrigeBoxFieldCreator cartrigeBoxFieldCreator, GameButton takeButton)
    {
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));
        _addButton = addButton ? addButton : throw new ArgumentNullException(nameof(addButton));

        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _timeDisplay = timeDisplay ? timeDisplay : throw new ArgumentNullException(nameof(timeDisplay));

        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));
        _takeButton = takeButton ? takeButton : throw new ArgumentNullException(nameof(takeButton));
    }

    public void Prepare()
    {
        SubscribeToCartrigeBoxFillerCreator();
        SubscribeToAddButton();

        PrepareGlobalStopwatch();

        SubscribeToCartrigeBoxFieldCreator();
        SubscribeToTakeButton();
    }

    public void Disable()
    {
        UnsubscribeFromCartrigeBoxFillerCreator();
        UnsubscribeFromAddButton();

        UnsubscribeFromCartrigeBoxFieldCreator();
        UnsubscribeFromTakeButton();
    }

    // 1 ABILITY
    private void SubscribeToCartrigeBoxFillerCreator()
    {
        _cartrigeBoxFillerCreator.Created += OnCartrigeBoxFieldFillerCreated;
    }

    private void UnsubscribeFromCartrigeBoxFillerCreator()
    {
        _cartrigeBoxFillerCreator.Created -= OnCartrigeBoxFieldFillerCreated;
    }

    private void SubscribeToAddButton()
    {
        _addButton.Pressed += OnPressedAddButton;
    }

    private void UnsubscribeFromAddButton()
    {
        _addButton.Pressed -= OnPressedAddButton;
    }

    private void OnPressedAddButton()
    {
        if (_cartrigeBoxFieldFiller == null)
        {
            return;
        }

        _cartrigeBoxFieldFiller.AddAmountAddedCartrigeBoxes(_amountAdditionalCartrigeBox);
    }

    private void OnCartrigeBoxFieldFillerCreated(CartrigeBoxFieldFiller cartrigeBoxFieldFiller)
    {
        _cartrigeBoxFieldFiller = cartrigeBoxFieldFiller;
    }

    // 2 ABILITY
    private void PrepareGlobalStopwatch()
    {
        Stopwatch stopwatch = _stopwatchCreator.Create();
        _timeDisplay.Init(stopwatch);
        _timeDisplay.On();
        stopwatch.Start();
    }

    // 3 ABILITY
    private void SubscribeToCartrigeBoxFieldCreator()
    {
        _cartrigeBoxFieldCreator.Created += OnCartrigeBoxFieldCreated;
    }

    private void UnsubscribeFromCartrigeBoxFieldCreator()
    {
        _cartrigeBoxFieldCreator.Created -= OnCartrigeBoxFieldCreated;
    }

    private void OnCartrigeBoxFieldCreated(CartrigeBoxField cartrigeBoxField)
    {
        _cartrigeBoxField = cartrigeBoxField;
    }

    private void SubscribeToTakeButton()
    {
        _takeButton.Pressed += OnPressedTakeButton;
    }

    private void UnsubscribeFromTakeButton()
    {
        _takeButton.Pressed -= OnPressedTakeButton;
    }

    private void OnPressedTakeButton()
    {
        if (_cartrigeBoxField == null)
        {
            return;
        }

        for (int i = 0; i < _amountTakenCartrigeBox; i++)
        {
            _cartrigeBoxField.TryGetCartrigeBox(out CartrigeBox cartrigeBox);
            cartrigeBox.Destroy();
        }
    }

    // PATTERN
    private void SubscribeTo()
    {

    }

    private void UnsubscribeFrom()
    {

    }
}