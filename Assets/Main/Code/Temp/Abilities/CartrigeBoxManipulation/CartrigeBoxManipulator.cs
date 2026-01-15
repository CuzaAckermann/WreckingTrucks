using System;
using UnityEngine;

public class CartrigeBoxManipulator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CartrigeBoxManipulatorSettings _settings;

    [Header("Buttons")]
    [SerializeField] private GameButton _addButton;
    [SerializeField] private GameButton _takeButton;
    [SerializeField] private GameButton _switchButton;

    private Stopwatch _stopwatch;
    private DispencerCreator _dispencerCreator;

    private StopwatchWaitingState _waitingState;
    private Dispencer _dispencer;

    private bool _isActivated;

    private bool _isSubscribedToButtons;
    private bool _isSubscribedToCreator;
    private bool _isSubscribedToDispencer;

    public void Init(Stopwatch stopwatchForTaking,
                     DispencerCreator dispencerCreator)
    {
        _stopwatch = stopwatchForTaking ?? throw new ArgumentNullException(nameof(stopwatchForTaking));
        _dispencerCreator = dispencerCreator ?? throw new ArgumentNullException(nameof(dispencerCreator));

        OffButtons();

        _isActivated = false;

        _isSubscribedToButtons = false;
        
        _isSubscribedToCreator = false;

        _isSubscribedToDispencer = false;

        SubscribeToDispencerCreator();
    }

    private void OnEnable()
    {
        SubscribeToButtons();

        SubscribeToDispencerCreator();

        SubscribeToDispencer();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtons();

        UnsubscribeFromDispencerCreator();

        UnsubscribeFromDispencer();
    }

    private void SubscribeToButtons()
    {
        if (_isSubscribedToButtons == false)
        {
            _addButton.Pressed += OnPressedAddButton;
            _takeButton.Pressed += OnPressedTakeButton;
            _switchButton.Pressed += Switch;

            _isSubscribedToButtons = true;
        }
    }

    private void UnsubscribeFromButtons()
    {
        if (_isSubscribedToButtons)
        {
            _addButton.Pressed -= OnPressedAddButton;
            _takeButton.Pressed -= OnPressedTakeButton;
            _switchButton.Pressed -= Switch;

            _isSubscribedToButtons = false;
        }
    }

    private void OnPressedAddButton()
    {
        if (_dispencer == null)
        {
            return;
        }

        _dispencer.AddAmountAddedCartrigeBoxes(_settings.AmountForAdd);
    }

    private void OnPressedTakeButton()
    {
        if (_dispencer == null)
        {
            return;
        }

        for (int i = 0; i < _settings.AmountForTaking; i++)
        {
            if (_dispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
            {
                cartrigeBox.Destroy();
            }
        }
    }

    private void Switch()
    {
        if (_dispencer == null)
        {
            return;
        }

        _isActivated = _isActivated == false;

        if (_isActivated)
        {
            StartWaitingTakeCartrigeBoxes();
        }
        else
        {
            _waitingState.Exit();
        }
    }

    private void StartWaitingTakeCartrigeBoxes()
    {
        _waitingState = new StopwatchWaitingState(_stopwatch, _settings.TimeForTaking);
        _waitingState.Enter(TakeCartrigeBoxes);
    }

    private void TakeCartrigeBoxes()
    {
        _waitingState.Exit();

        for (int i = 0; i < _settings.AmountForTaking; i++)
        {
            if (_dispencer.TryGetCartrigeBox(out CartrigeBox cartrigeBox))
            {
                cartrigeBox.Destroy();
            }
            else
            {
                Logger.Log("CartrigeBoxField is empty");

                break;
            }
        }

        StartWaitingAddCartrigeBoxes();
    }

    private void StartWaitingAddCartrigeBoxes()
    {
        _waitingState = new StopwatchWaitingState(_stopwatch, _settings.TimeForAdd);

        _waitingState.Enter(AddCartrigeBoxes);
    }

    private void AddCartrigeBoxes()
    {
        _waitingState.Exit();

        _dispencer.AddAmountAddedCartrigeBoxes(_settings.AmountForAdd);

        StartWaitingTakeCartrigeBoxes();
    }

    private void SubscribeToDispencerCreator()
    {
        if (_isSubscribedToCreator == false && _dispencerCreator != null)
        {
            _dispencerCreator.Created += OnCartrigeBoxDispencerCreated;

            _isSubscribedToCreator = true;
        }
    }

    private void UnsubscribeFromDispencerCreator()
    {
        if (_isSubscribedToCreator)
        {
            _dispencerCreator.Created -= OnCartrigeBoxDispencerCreated;

            _isSubscribedToCreator = false;
        }
    }

    private void OnCartrigeBoxDispencerCreated(Dispencer cartrigeBoxDispencer)
    {
        _dispencer = cartrigeBoxDispencer;

        SubscribeToDispencer();
        OnButtons();
    }

    private void SubscribeToDispencer()
    {
        if (_isSubscribedToDispencer == false && _dispencer != null)
        {
            _dispencer.Cleared += OffButtons;

            _isSubscribedToDispencer = true;
        }
    }

    private void UnsubscribeFromDispencer()
    {
        if (_isSubscribedToDispencer)
        {
            _dispencer.Cleared -= OffButtons;

            _isSubscribedToDispencer = false;
        }
    }

    private void OnButtons()
    {
        _addButton.On();
        _takeButton.On();
        _switchButton.On();
    }

    private void OffButtons()
    {
        if (_dispencer != null)
        {
            _dispencer.Cleared -= OffButtons;
            _isSubscribedToDispencer = false;
        }

        _addButton.Off();
        _takeButton.Off();
        _switchButton.Off();

        _dispencer = null;
    }
}