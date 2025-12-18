using System;

public class TesterAbilities
{
    private CartrigeBoxManipulatorSettings _cartrigeBoxFieldSettings;

    //1 ABILITY ADD CARTRIGE BOX
    private CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;
    private GameButton _addButton;

    private CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    //2 DISPLAY GLOBAL STOPWATCH

    private StopwatchCreator _stopwatchCreator;
    private TimeDisplay _timeDisplay;

    //3 ABILITY REMOVE CARTRIGE BOX

    private CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;
    private GameButton _takeButton;

    private CartrigeBoxField _cartrigeBoxField;

    //4 CARTRIGE BOX MANIPULATOR
    private CartrigeBoxManipulator _cartrigeBoxManipulator;
    private GameButton _switchButton;

    private bool _isActivatedManipulator;

    public TesterAbilities(CartrigeBoxManipulatorSettings cartrigeBoxFieldSettings,
                           CartrigeBoxFillerCreator cartrigeBoxFillerCreator, GameButton addButton,
                           StopwatchCreator stopwatchCreator, TimeDisplay timeDisplay,
                           CartrigeBoxFieldCreator cartrigeBoxFieldCreator, GameButton takeButton,
                           CartrigeBoxManipulator cartrigeBoxManipulator, GameButton switchButton)
    {
        _cartrigeBoxFieldSettings = cartrigeBoxFieldSettings ? cartrigeBoxFieldSettings : throw new ArgumentNullException(nameof(cartrigeBoxFieldSettings));

        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));
        _addButton = addButton ? addButton : throw new ArgumentNullException(nameof(addButton));

        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _timeDisplay = timeDisplay ? timeDisplay : throw new ArgumentNullException(nameof(timeDisplay));

        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));
        _takeButton = takeButton ? takeButton : throw new ArgumentNullException(nameof(takeButton));

        _cartrigeBoxManipulator = cartrigeBoxManipulator ?? throw new ArgumentNullException(nameof(cartrigeBoxManipulator));
        _switchButton = switchButton ? switchButton : throw new ArgumentNullException(nameof(switchButton));
        _isActivatedManipulator = false;
    }

    public void Prepare()
    {
        SubscribeToCartrigeBoxFillerCreator();
        SubscribeToAddButton();

        PrepareGlobalStopwatch();

        SubscribeToCartrigeBoxFieldCreator();
        SubscribeToTakeButton();

        SubscribeToSwitchButton();
    }

    public void Disable()
    {
        UnsubscribeFromCartrigeBoxFillerCreator();
        UnsubscribeFromAddButton();

        UnsubscribeFromCartrigeBoxFieldCreator();
        UnsubscribeFromTakeButton();

        UnsubscribeFromSwitchButton();
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

        _cartrigeBoxFieldFiller.AddAmountAddedCartrigeBoxes(_cartrigeBoxFieldSettings.AmountForAdd);
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

        for (int i = 0; i < _cartrigeBoxFieldSettings.AmountForTaking; i++)
        {
            _cartrigeBoxField.TryGetCartrigeBox(out CartrigeBox cartrigeBox);
            cartrigeBox.Destroy();
        }
    }

    private void SubscribeToSwitchButton()
    {
        _switchButton.Pressed += SwitchManipulator;
    }

    private void UnsubscribeFromSwitchButton()
    {
        _switchButton.Pressed -= SwitchManipulator;
    }

    private void SwitchManipulator()
    {
        _isActivatedManipulator = _isActivatedManipulator == false;

        if (_isActivatedManipulator)
        {
            _cartrigeBoxManipulator.Start();
        }
        else
        {
            _cartrigeBoxManipulator.Stop();
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