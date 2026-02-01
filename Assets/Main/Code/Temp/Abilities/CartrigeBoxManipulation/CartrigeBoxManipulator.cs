using System;
using UnityEngine;

public class CartrigeBoxManipulator : MonoBehaviour, ICommandCreator
{
    [Header("Settings")]
    [SerializeField] private CartrigeBoxManipulatorSettings _settings;

    [Header("Buttons")]
    [SerializeField] private GameButton _addButton;
    [SerializeField] private GameButton _takeButton;
    [SerializeField] private GameButton _switchButton;

    private Dispencer _dispencer;

    private EventBus _eventBus;

    private Command _currentCommand;

    private bool _isActivated;

    private bool _isSubscribedToButtons;
    private bool _isSubscribedToDispencer;

    private bool _isInited;

    public void Init(EventBus eventBus)
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

        OffButtons();

        _isActivated = false;

        _isSubscribedToButtons = false;
        
        _isSubscribedToDispencer = false;

        SubscribeToButtons();

        _eventBus?.Subscribe<CreatedSignal<Dispencer>>(SetDispencer);

        SubscribeToDispencer();

        _isInited = true;
    }

    public event Action<IDestroyable> DestroyedIDestroyable;

    public event Action<Command> CommandCreated;

    private void OnEnable()
    {
        if (_isInited == false)
        {
            return;
        }

        SubscribeToButtons();

        _eventBus?.Subscribe<CreatedSignal<Dispencer>>(SetDispencer);

        SubscribeToDispencer();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtons();

        _eventBus?.Unsubscribe<CreatedSignal<Dispencer>>(SetDispencer);

        UnsubscribeFromDispencer();
    }

    public void Destroy()
    {
        DestroyedIDestroyable?.Invoke(this);
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
            SendCommand(TakeCartrigeBoxes, _settings.TimeForTaking);
        }
        else
        {
            CancelCommand();
        }
    }

    private void TakeCartrigeBoxes()
    {
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

        SendCommand(AddCartrigeBoxes, _settings.TimeForAdd);
    }

    private void AddCartrigeBoxes()
    {
        _dispencer.AddAmountAddedCartrigeBoxes(_settings.AmountForAdd);

        SendCommand(TakeCartrigeBoxes, _settings.TimeForTaking);
    }

    private void SetDispencer(CreatedSignal<Dispencer> createdDispencerSignal)
    {
        _dispencer = createdDispencerSignal.Creatable;

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

    private void SendCommand(Action action, float delay)
    {
        _currentCommand = new Command(action, delay);

        CommandCreated?.Invoke(_currentCommand);
    }

    private void CancelCommand()
    {
        _currentCommand?.Cancel();

        _currentCommand = null;
    }
}