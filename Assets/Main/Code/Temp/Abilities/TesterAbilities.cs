using System;
using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    [SerializeField] private CartrigeBoxManipulatorSettings _cartrigeBoxFieldSettings;

    //1 ABILITY ADD CARTRIGE BOX
    [SerializeField] private GameButton _addButton;
    private CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;

    private CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    //2 DISPLAY GLOBAL STOPWATCH

    [SerializeField] private TimeDisplay _timeDisplay;
    private StopwatchCreator _stopwatchCreator;

    //3 ABILITY REMOVE CARTRIGE BOX

    [SerializeField] private GameButton _takeButton;
    private CartrigeBoxFieldCreator _cartrigeBoxFieldCreator;

    private CartrigeBoxField _cartrigeBoxField;

    //4 CARTRIGE BOX MANIPULATOR
    [SerializeField] private GameButton _switchButton;
    private CartrigeBoxManipulator _cartrigeBoxManipulator;

    private bool _isActivatedManipulator;

    //5 CURRENT DELTA TIME
    [SerializeField] private RoundedAmountDisplay _deltaTimeDisplay;
    private DeltaTimeCoefficientDefiner _deltaTimeCoefficientDefiner;

    private bool _isInitialized = false;

    public void Init(CartrigeBoxFillerCreator cartrigeBoxFillerCreator,
                     StopwatchCreator stopwatchCreator,
                     CartrigeBoxFieldCreator cartrigeBoxFieldCreator,
                     DeltaTimeCoefficientDefiner deltaTimeCoefficientDefiner)
    {
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));

        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));

        _cartrigeBoxFieldCreator = cartrigeBoxFieldCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFieldCreator));

        _deltaTimeCoefficientDefiner = deltaTimeCoefficientDefiner ?? throw new ArgumentNullException(nameof(deltaTimeCoefficientDefiner));

        _cartrigeBoxManipulator = new CartrigeBoxManipulator(_cartrigeBoxFieldSettings,
                                                             _stopwatchCreator.Create(),
                                                             _cartrigeBoxFieldCreator,
                                                             _cartrigeBoxFillerCreator);

        _addButton.Off();
        _takeButton.Off();
        _switchButton.Off();
        _deltaTimeDisplay.Off();

        _isActivatedManipulator = false;

        _isInitialized = true;
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
        {
            return;
        }

        Enable();
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
        {
            return;
        }

        Disable();
    }

    public void Enable()
    {
        SubscribeToCartrigeBoxFillerCreator();
        SubscribeToAddButton();

        PrepareGlobalStopwatch();

        SubscribeToCartrigeBoxFieldCreator();
        SubscribeToTakeButton();

        SubscribeToSwitchButton();

        PrepareDeltaTimeDisplay();
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
        _addButton.On();
    }

    // 2 ABILITY
    private void PrepareGlobalStopwatch()
    {
        Stopwatch stopwatch = _stopwatchCreator.Create();
        _timeDisplay.Init(stopwatch);
        _timeDisplay.On();
        stopwatch.Start();
    }

    // 3 and 4 ABILITY
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
        _takeButton.On();
        _switchButton.On();
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

    //5 ABILITY
    private void PrepareDeltaTimeDisplay()
    {
        _deltaTimeDisplay.Init(_deltaTimeCoefficientDefiner);
        _deltaTimeDisplay.On();
    }

    // PATTERN
    private void SubscribeTo()
    {

    }

    private void UnsubscribeFrom()
    {

    }
}