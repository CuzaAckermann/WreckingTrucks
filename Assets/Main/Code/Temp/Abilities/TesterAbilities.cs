using System;
using UnityEngine;

public class TesterAbilities : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private TimeDisplay _timeDisplay;
    [SerializeField] private RoundedAmountDisplay _deltaTimeDisplay;

    [Header("CartrigeBoxField Manipulation")]
    [SerializeField] private GameButton _addButton;
    [SerializeField] private GameButton _takeButton;
    [SerializeField] private CartrigeBoxManipulatorSettings _cartrigeBoxFieldSettings;
    [SerializeField] private GameButton _switchButton;


    private StopwatchCreator _stopwatchCreator;
    private DeltaTimeCoefficientDefiner _deltaTimeCoefficientDefiner;

    private DispencerCreator _cartrigeBoxDispencerCreator;
    private CartrigeBoxFillerCreator _cartrigeBoxFillerCreator;

    private Dispencer _cartrigeBoxDispencer;
    private CartrigeBoxFieldFiller _cartrigeBoxFieldFiller;

    private CartrigeBoxManipulator _cartrigeBoxManipulator;
    private bool _isActivatedManipulator;

    private bool _isInitialized = false;

    public void Init(StopwatchCreator stopwatchCreator,
                     DeltaTimeCoefficientDefiner deltaTimeCoefficientDefiner,
                     DispencerCreator cartrigeBoxDispencerCreator,
                     CartrigeBoxFillerCreator cartrigeBoxFillerCreator)
    {
        _stopwatchCreator = stopwatchCreator ?? throw new ArgumentNullException(nameof(stopwatchCreator));
        _deltaTimeCoefficientDefiner = deltaTimeCoefficientDefiner ?? throw new ArgumentNullException(nameof(deltaTimeCoefficientDefiner));

        _cartrigeBoxDispencerCreator = cartrigeBoxDispencerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));
        _cartrigeBoxFillerCreator = cartrigeBoxFillerCreator ?? throw new ArgumentNullException(nameof(cartrigeBoxFillerCreator));

        _cartrigeBoxManipulator = new CartrigeBoxManipulator(_cartrigeBoxFieldSettings,
                                                             _stopwatchCreator.Create(),
                                                             _cartrigeBoxDispencerCreator,
                                                             _cartrigeBoxFillerCreator);

        _deltaTimeDisplay.Off();

        _addButton.Off();
        _takeButton.Off();
        _switchButton.Off();

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

    public void PrepareTimeDisplay()
    {
        PrepareGlobalStopwatch();
        PrepareDeltaTimeDisplay();
    }

    public void Enable()
    {
        SubscribeToCartrigeBoxFieldCreator();
        SubscribeToCartrigeBoxFillerCreator();

        SubscribeToAddButton();
        SubscribeToTakeButton();

        SubscribeToSwitchButton();
    }

    public void Disable()
    {
        UnsubscribeFromCartrigeBoxFieldCreator();
        UnsubscribeFromCartrigeBoxFillerCreator();

        UnsubscribeFromAddButton();
        UnsubscribeFromTakeButton();

        UnsubscribeFromSwitchButton();
    }

    private void PrepareGlobalStopwatch()
    {
        Stopwatch stopwatch = _stopwatchCreator.Create();
        _timeDisplay.Init(stopwatch);
        _timeDisplay.On();
        stopwatch.Start();
    }

    private void PrepareDeltaTimeDisplay()
    {
        _deltaTimeDisplay.Init(_deltaTimeCoefficientDefiner);
        _deltaTimeDisplay.On();
    }

    private void SubscribeToCartrigeBoxFieldCreator()
    {
        _cartrigeBoxDispencerCreator.Created += OnCartrigeBoxDispencerCreated;
    }

    private void UnsubscribeFromCartrigeBoxFieldCreator()
    {
        _cartrigeBoxDispencerCreator.Created -= OnCartrigeBoxDispencerCreated;
    }

    private void OnCartrigeBoxDispencerCreated(Dispencer cartrigeBoxDispencer)
    {
        _cartrigeBoxDispencer = cartrigeBoxDispencer;
        _takeButton.On();
        _switchButton.On();
    }

    private void SubscribeToCartrigeBoxFillerCreator()
    {
        _cartrigeBoxFillerCreator.Created += OnCartrigeBoxFieldFillerCreated;
    }

    private void UnsubscribeFromCartrigeBoxFillerCreator()
    {
        _cartrigeBoxFillerCreator.Created -= OnCartrigeBoxFieldFillerCreated;
    }

    private void OnCartrigeBoxFieldFillerCreated(CartrigeBoxFieldFiller cartrigeBoxFieldFiller)
    {
        _cartrigeBoxFieldFiller = cartrigeBoxFieldFiller;
        _addButton.On();
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

        _cartrigeBoxDispencer.AddAmountAddedCartrigeBoxes(_cartrigeBoxFieldSettings.AmountForAdd);
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
        if (_cartrigeBoxDispencer == null)
        {
            return;
        }

        for (int i = 0; i < _cartrigeBoxFieldSettings.AmountForTaking; i++)
        {
            if (_cartrigeBoxDispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
            {
                cartrigeBox.Destroy();
            }
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